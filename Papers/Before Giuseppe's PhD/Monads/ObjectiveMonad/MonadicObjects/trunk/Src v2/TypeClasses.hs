{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

class Monad (st h) => Env st h ref where
  type New h :: * -> *
  add :: h -> a -> New h a
  del :: New h a -> h

  eval :: ref h a -> st h a
  (=:) :: ref h a -> a -> st h ()
  (*=) :: ref h a -> (a->a) -> st h ()

  (>>+) :: a -> (ref (New h a) a -> st (New h a) b) -> st h b


ex1 :: Env st h ref => ref h Int -> st h Int
ex1 i = do  i *= (+2)
            v' <- eval i
            return v'

class Subtype a b where downcast :: a -> b
instance Subtype a a where downcast x = x
instance (Subtype a b, Subtype b c) => Subtype a c where downcast = (downcast :: b -> c) . (downcast :: a -> b)


ex2 :: forall st h0 h1 ref . (h1 ~ New h0 Int, Monad (st h1), Env st h0 ref) => st h0 String
ex2 = do 10 >>+ (\(i :: ref (New h0 Int) Int) -> return "")