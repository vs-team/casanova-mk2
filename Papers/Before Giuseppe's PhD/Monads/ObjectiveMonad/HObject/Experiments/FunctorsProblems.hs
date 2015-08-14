{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

class C a a'  where convert :: a -> a'
class F a b   where apply :: a -> b
class S s a   where select :: s -> a

data CInt = CInt Int

instance S (Int,String) Int where select (i,_) = i
instance F Int CInt where apply = CInt

f :: forall s a b . (S s a, F a b) => s -> b
f s = 
  let v = select s :: a
      y = apply v :: b
  in y

x :: Int
x = f (10,"Pippo")

