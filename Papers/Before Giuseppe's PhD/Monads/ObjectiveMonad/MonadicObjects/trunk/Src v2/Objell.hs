{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

import Prelude hiding (lookup,read,last)

{-
State ref and stack
-}
class Memory m where
  data Malloc :: * -> * -> *
  malloc :: m -> a -> Malloc a m
  free   :: Malloc a m -> m
  read   :: Malloc a m -> a
  write  :: a -> Malloc a m -> Malloc a m

class (Memory m,Monad (st m m)) => State st m where
  data Ref st :: * -> * -> *
  eval :: Ref st m a -> st m m a
  (.=) :: Ref st m a -> a -> st m m ()
  delete :: st (Malloc a m) m ()
  new :: a -> st m (Malloc a m) (Ref st (Malloc a m) a)
  (>>+) :: (Memory m', Memory m'') => st m m' a -> (a -> st m' m'' b) -> st m m'' b

(*=) :: State st s => Ref st s a -> (a->a) -> st s s ()
ref *= f = do v <- eval ref
              ref .= (f v)

{-
Records
-}
class (State st m) => Record r st m where
  data Label st r :: * -> *
  (<==) :: (Ref st) m r -> Label st r a -> (Ref st) m a


{-
Mutable Implementation
-}

{- HList -}
data Nil = Nil deriving (Show)

class HList l
instance HList Nil
instance HList tl => HList (Malloc h tl)

data Z = Z
data S n = S n

class CNum n
instance CNum Z
instance CNum n => CNum (S n)

type family HLength l :: *
type instance HLength Nil = Z
type instance HLength (Malloc h tl) = S (HLength tl)

type family HAt l n :: *
type instance HAt (Malloc h tl) Z = h
type instance HAt (Malloc h tl) (S n) = HAt tl n

class (HList l, CNum n) => HLookup l n where
  lookup :: l -> n -> HAt l n
  update :: l -> n -> HAt l n -> l

instance (HList tl) => HLookup (Malloc h tl) Z where
  lookup (Malloc h tl) _ = h
  update (Malloc h tl) _ h' = (Malloc h' tl)

instance (HList tl, CNum n, HLookup tl n) => HLookup (Malloc h tl) (S n) where
  lookup (Malloc _ tl) _ = lookup tl (undefined::n)
  update (Malloc h tl) _ v' = (Malloc h (update tl (undefined::n) v'))


{- State Monad Implementation -}
data St s s' a = St(s->(a,s'))

type Get s a = St s s a
type Set s a = a -> St s s ()

instance Monad (St s s) where
  return x = St(\s -> (x,s))
  (St st) >>= k = St(\s -> 
                          let (res,s') = st s 
                              (St k') = k res
                          in k' s')

runSt :: St s s' a -> s -> a
runSt (St st) s = fst (st s)


{- Memory and State Implementation -}
instance (HList m) => Memory m where
  data Malloc a m = Malloc a m deriving (Show)
  malloc m a = Malloc a m
  free (Malloc h tl) = tl
  read   (Malloc h tl) = h
  write  h' (Malloc h tl) = Malloc h' tl


instance (HList m, n~HLength m) => State St m where
  data Ref St m a = StRef (Get m a) (Set m a)
  eval (StRef get set) = get
  (StRef get set) .= v = set v
  delete = St(\s -> ((), free s))
  new v = let new_ref = StRef (St (\s -> (read s, s)))
                              (\v' -> St(\s -> ((), write v' s)))
          in St (\s -> (new_ref, malloc s v))
  (St st) >>+ k = St(\s -> 
                            let (res,s') = st s 
                                (St k') = k res
                            in k' s')

{- State Example -}

--ex1 :: forall m0 m1 . (m0 ~ Nil, m1 ~ (Malloc(m0,Int))) => St m0 m1 Int
ex1 = new 10 >>+ (\i -> do i *= (+2)
                           eval i)

res1 :: Int
res1 = runSt ex1 Nil


{- Record Implementation -}
instance (HList m, HList r) => Record r St m where
  data Label St r a = StLabel (r->a) (r->a->r)
  StRef get set <== StLabel read write =
    StRef(do r <- get
             return (read r))
         (\v'-> do r <- get
                   set (write r v'))

labelAt :: forall l n . (HList l, CNum n, HLookup l n) => n -> Label St l (HAt l n)
labelAt _ = StLabel (\l -> lookup l (undefined::n)) 
                    (\l -> \v -> update l (undefined::n) v)


{- Record Example -}
infixr `Malloc`
type Person = String `Malloc` String `Malloc` Int `Malloc` Nil
first :: Label St Person String
first = labelAt Z
last :: Label St Person String
last = labelAt (S Z)
age :: Label St Person Int
age = labelAt (S (S Z))

mk_person :: String -> String -> Int -> Person
mk_person f l a = (f `Malloc` l `Malloc` a `Malloc` Nil)

ex2 = new (mk_person "John" "Smith" 27) >>+ (\p -> do (p <== last) *= (++ " Jr.")
                                                      (p <== age) .= 25
                                                      eval p)

res2 = runSt ex2 Nil
