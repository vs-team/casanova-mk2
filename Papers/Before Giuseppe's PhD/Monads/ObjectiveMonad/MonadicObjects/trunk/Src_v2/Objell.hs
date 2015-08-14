{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

import Prelude hiding (lookup,read,last,(<=),const)

{-
State ref and stack
-}
class Memory m where
  data Malloc :: * -> * -> *
  malloc :: m -> a -> Malloc a m
  free   :: Malloc a m -> m
  read   :: Malloc a m -> a
  write  :: a -> Malloc a m -> Malloc a m

class State st where
  data Ref st :: * -> * -> *
  eval :: (Memory m) => Ref st m a -> st m m a
  (.=) :: (Memory m) => Ref st m a -> a -> st m m ()
  delete :: (Memory m) => st (Malloc a m) m ()
  new :: (Memory m) => a -> st m (Malloc a m) (Ref st (Malloc a m) a)
  const :: (Memory m) => a -> st m m a
  (>>>=) :: (Memory m, Memory m', Memory m'') => st m m' a -> (a -> st m' m'' b) -> st m m'' b
  (>>>) :: (Memory m, Memory m', Memory m'') => st m m' a -> st m' m'' b -> st m m'' a

instance (State st, Memory m) => Monad (st m m) where
  return = const
  (>>=) = (>>>=)

(*=) :: (State st, Memory s) => Ref st s a -> (a->a) -> st s s ()
ref *= f = do v <- eval ref
              ref .= (f v)

{-
Records
-}
class (State st) => Record r st where
  data Label st r :: * -> *
  data Field st :: * -> *
  (<==) :: Memory m => (Ref st) m r -> Label st r (Field st a) -> (Ref st) m a


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

runSt :: St s s' a -> s -> a
runSt (St st) s = fst (st s)


{- Memory and State Implementation -}
instance (HList m) => Memory m where
  data Malloc a m = Malloc a m deriving (Show)
  malloc m a = Malloc a m
  free (Malloc h tl) = tl
  read   (Malloc h tl) = h
  write  h' (Malloc h tl) = Malloc h' tl


instance State St where
  data Ref St m a = StRef (Get m a) (Set m a)
  eval (StRef get set) = get
  (StRef get set) .= v = set v
  delete = St(\s -> ((), free s))
  new v = let new_ref = StRef (St (\s -> (read s, s)))
                              (\v' -> St(\s -> ((), write v' s)))
          in St (\s -> (new_ref, malloc s v))
  const x = St(\s -> (x,s))
  (St st) >>>= k = St(\s -> 
                            let (res,s') = st s 
                                (St k') = k res
                            in k' s')
  (St st) >>> (St st') = St(\s -> 
                                 let (res,s') = st s 
                                     (res',s'') = st' s'
                                 in (res,s''))

{- State Example -}

ex1 :: (HList m0, m1 ~ (Malloc Int m0), State st) => st m0 m1 Int
ex1 = new 10 >>>= (\i -> do i *= (+2)
                            eval i)

res1 = runSt ex1 Nil

ex1' :: (HList m0, m1 ~ (Malloc Int m0), State st) => st m0 m0 Int
ex1' = new 10 >>>= (\i -> do i *= (+2)
                             eval i) >>> delete

res1' = runSt ex1' Nil


{- Record Implementation -}
instance (HList r) => Record r St where
  data Label St r a = StLabel (r->a) (r->a->r)
  data Field St a = StField a deriving (Show)
  StRef get set <== StLabel read write =
    StRef(do r <- get
             return (let (StField f) = (read r) in f))
         (\v'-> do r <- get
                   set (write r (StField v')))

labelAt :: forall l n . (HList l, CNum n, HLookup l n) => n -> Label St l (HAt l n)
labelAt _ = StLabel (\l -> lookup l (undefined::n)) 
                    (\l -> \v -> update l (undefined::n) v)


{- Coercive Subtyping -}
class Coercible a b where
  coerce :: a -> b

{-
instance Coercible a a where
  coerce a = a
-}

{-
instance (Coercible a b, Coercible b c) => Coercible a c where
  coerce a = (coerce :: b -> c) ((coerce :: a -> b) a)
-}

{- Coercion and References -}
instance HList tl => Coercible (Ref St tl a) (Ref St (Malloc h tl) a) where
  coerce ref =
    StRef (St(\(Malloc h tl) ->
                          let (res, tl') = get ref tl
                          in (res, h `Malloc` tl')))
          (\v -> St(\(Malloc h tl) -> 
            let ((),tl') = set ref tl v
            in ((),h `Malloc` tl')))
    where get (StRef (St g) _) = g
          set (StRef _ s) = \st -> \v -> 
                                         let (St s') = s v
                                         in s' st

{-
ex3 = do+ i <- new 10 :: Ref (New Nil Int) Int
          s <- new "Hello" :: Ref (New (New Nil Int) String) String
          (coerce i) *= (+2)
          s *= (++" World")
          return ())
-}

ex3 :: forall m0 m1 m2 st . (HList m0, State st, m1 ~ Malloc Int m0, m2 ~ Malloc String m1,
                             Coercible (Ref st m1 Int) (Ref st m2 Int)) => st m0 m2 String
ex3 = new 10 >>>= (\i -> new "Hello" >>>= (\s -> do ((coerce i) :: Ref st m2 Int) *= (+2)
                                                    s *= (++ " World")
                                                    eval s))

res3 = runSt ex3 Nil


{- Objects -}
class Recursive o where
  type Rec o :: *
  to :: o -> Rec o
  from :: Rec o -> o

class (Record o st, Recursive o) => Object o st where
  data Method st :: * -> * -> * -> *
  (<=|) :: Memory s => Ref st s o -> Label st o (Method st (Rec o) a b) -> a -> st s s b
  mk_method :: (Ref st o o -> a -> st o o b) -> Method st (Rec o) a b

class Object o st => Inherits o b st where
  data Inherit :: * -> *
  get_base :: Memory s => Ref st s o -> Ref st s b

instance (Recursive o, Record o St) => Object o St where
  data Method St ro a b = StMethod(ro -> a -> (b,ro))
  self_ref <=| (StLabel read write) = 
             \x -> do self <- eval self_ref
                      let (StMethod m) = read self
                          (res,self' :: Rec o) = m (to self) x
                      self_ref .= (from self')
                      return res
  mk_method m = StMethod(\this -> \args -> let (St res_st) = m id_ref args
                                               (res,this' :: o) = res_st ((from this) :: o)
                                           in (res,((to this') :: Rec o)))
                where id_ref = StRef (St (\s -> (s,s))) (\s' -> (St (\s -> ((),s'))))

instance (Object o St, o ~ (Malloc (Inherit b) so)) => Inherits o b St where
  data Inherit a = StInherit a
  get_base self_ref =   StRef(do ((StInherit base) `Malloc` tl) <- eval self_ref
                                 return base)
                             (\base' -> do (_ `Malloc` tl) <- eval self_ref
                                           self_ref .= ((StInherit base') `Malloc` tl)
                                           return ())


instance (Inherits o b st, Memory s) => Coercible (Ref st s o) (Ref st s b) where
  coerce = get_base

class Memory m => Selectable st m t a where
  type Selection st m t a :: *
  (<=) :: Ref st m t -> Label st t a -> Selection st m t a

instance (Record r st, Memory m) => Selectable st m r (Field st a) where
  type Selection st m r (Field st a) = Ref st m a
  (<=) = (<==)

instance (Object o st, ro~Rec o, Memory m) => Selectable st m o (Method st ro a b) where
  type Selection st m o (Method st ro a b) = a -> st m m b
  (<=) = (<=|)

{-
instance (Inherits o b st, Selectable st m (Base o) a, Memory m) => Selectable st m o a where
  type Selection st m o a = Selection st m (Base o) a
  (<=) = get_base . (<=)
-}

{- Record Example: Person -}
infixr `Malloc`
type Person = (Field St String) `Malloc` (Field St String) `Malloc` (Field St Int) `Malloc` Nil
first :: Label St Person (Field St String)
first = labelAt Z
last :: Label St Person (Field St String)
last = labelAt (S Z)
age :: Label St Person (Field St Int)
age = labelAt (S (S Z))

mk_person :: String -> String -> Int -> Person
mk_person f l a = ((StField f) `Malloc` (StField l) `Malloc` (StField a) `Malloc` Nil)

ex2 :: (HList m0, State st, m1 ~ Malloc Person m0, Record Person st) => 
            Label st Person (Field st String) -> Label st Person (Field st Int) -> st m0 m1 Person
ex2 last age = new (mk_person "John" "Smith" 27) >>>= (\p -> do (p <= last) *= (++ " Jr.")
                                                                (p <= age) .= 25
                                                                eval p)

res2 = runSt (ex2 last age) Nil


{- Object Example: Vectors -}
type Vector2Def k = Field St Float `Malloc` Field St Float `Malloc` Method St k () () `Malloc` Method St k () Float `Malloc` Nil
data RecVector2 = RecVector2 (Vector2Def RecVector2)
type Vector2 = Vector2Def RecVector2
instance Recursive Vector2 where
  type Rec Vector2 = RecVector2
  to = RecVector2
  from (RecVector2 v) = v
x :: Label St Vector2 (Field St Float)
x = labelAt Z
y :: Label St Vector2 (Field St Float)
y = labelAt (S Z)
norm2 :: Label St Vector2 (Method St (Rec Vector2) () ())
norm2 = labelAt (S (S Z))
len2 :: Label St Vector2 (Method St (Rec Vector2) () Float)
len2 = labelAt (S (S (S Z)))

mk_vector2 :: Float -> Float -> Vector2
mk_vector2 xv yv = (StField xv) `Malloc` (StField yv) `Malloc` norm `Malloc` len `Malloc` Nil
                    where norm = mk_method (\this -> \() -> do l <- (this <= len2) ()
                                                               (this <= x) *= (/ l)
                                                               (this <= y) *= (/ l))
                          len = mk_method (\this -> \() -> do xv <- eval (this <= x)
                                                              yv <- eval (this <= y)
                                                              return (sqrt(xv * xv + yv * yv)))

type Vector3Def k = Inherit Vector2 `Malloc` Field St Float `Malloc` Method St k () () `Malloc` Method St k () Float `Malloc` Nil
data RecVector3 = RecVector3 (Vector3Def RecVector3)
type Vector3 = Vector3Def RecVector3
instance Recursive Vector3 where
  type Rec Vector3 = RecVector3
  to = RecVector3
  from (RecVector3 v) = v
z :: Label St Vector3 (Field St Float)
z = labelAt (S Z)
norm3 :: Label St Vector3 (Method St (Rec Vector3) () ())
norm3 = labelAt (S (S Z))
len3 :: Label St Vector3 (Method St (Rec Vector3) () Float)
len3 = labelAt (S (S (S Z)))

mk_vector3 :: Float -> Float -> Float -> Vector3
mk_vector3 xv yv zv = StInherit (mk_vector2 xv yv) `Malloc` StField zv `Malloc` norm `Malloc` len `Malloc` Nil
                      where norm = mk_method (\this -> \() -> do l <- (this <= len3) ()
                                                                 ((get_base this) <= x) *= (/l)
                                                                 ((get_base this) <= y) *= (/l)
                                                                 (this <= z) *= (/l)) 
                            len = mk_method (\this -> \() -> do xv <- eval ((get_base this) <= x)
                                                                yv <- eval ((get_base this) <= y)
                                                                zv <- eval (this <= z)
                                                                return (sqrt(xv * xv + yv * yv + zv * zv)))

ex4 :: forall mem st . (State st, mem ~ (Vector3 `Malloc` Nil), Object Vector3 st, Object Vector2 st, Inherits Vector3 Vector2 st) => 
                  Label st Vector2 (Field st Float) -> Label st Vector2 (Method st (Rec Vector2) () ()) -> 
                  Label st Vector3 (Method st (Rec Vector3) () ()) -> st Nil Nil Bool
ex4 x norm2 norm3 = new (mk_vector3 0.0 2.0 (-1.0)) >>>= (\v -> do xv <- eval ((get_base v) <= x)
                                                                   let v' = coerce v :: Ref st mem Vector2
                                                                   xv' <- eval (v' <= x)
                                                                   (v' <= norm2)()
                                                                   (v <= norm3)()
                                                                   return (xv == xv')) >>> delete

res4 :: Bool
res4 = runSt (ex4 x norm2 norm3) Nil


type CounterDef k = Field St Int `Malloc` Method St k () () `Malloc` Method St k () () `Malloc` Nil
data RecCounter = RecCounter (CounterDef RecCounter)
type Counter = CounterDef RecCounter
instance Recursive Counter where
  type Rec Counter = RecCounter
  to = RecCounter
  from (RecCounter v) = v
val :: Label St Counter (Field St Int)
val = labelAt Z
incr :: Label St Counter (Method St (Rec Counter) () ())
incr = labelAt (S Z)
faster :: Label St Counter (Method St (Rec Counter) () ())
faster = labelAt (S (S Z))

mk_counter :: Int -> Counter
mk_counter v =  StField v `Malloc` (incr 1) `Malloc` faster `Malloc` Nil
                where incr k = mk_method (\this -> \() -> do (this <= val) *= (+k)) 
                      faster = mk_method (\(this :: Ref St Counter Counter)-> \() -> do v <- eval (this <= val)
                                                                                        this .= (StField v `Malloc` (incr 2) `Malloc` faster `Malloc` Nil)
                                                                                        return ())

ex5 = new (mk_counter 0) >>>= (\c -> do (c <= incr) ()
                                        (c <= incr) ()
                                        (c <= faster) ()
                                        (c <= incr) ()
                                        v <- eval (c <= val)
                                        return v) >>> delete

res5 = runSt ex5 Nil
