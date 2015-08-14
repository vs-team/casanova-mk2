{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

class W a b   where w :: a -> b
class S a b   where s :: a -> b
class (S a (Intermediate a c), W (Intermediate a c) c) => SandW a c where type Intermediate a c :: *

data CInt = CInt Int deriving Show

instance S (Int,String) Int where s (i,_) = i
instance W Int CInt where w i = CInt(i+1)
instance SandW (Int,String) CInt where type Intermediate (Int,String) CInt = Int

f :: forall a b . (SandW a b) => a -> b
f x = 
  let y = s x :: Intermediate a b
      z = w y :: b
  in z


x :: CInt
x = f (10::Int,"Pippo")
