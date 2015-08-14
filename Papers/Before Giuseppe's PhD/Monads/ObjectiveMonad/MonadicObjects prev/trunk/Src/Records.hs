{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

{- In this module we define the basic building blocks -}

module Records where

  data EmptyRecord = EmptyRecord deriving (Show, Eq)
  data AddField e l = AddField e l deriving (Show, Eq)

  infixr 8 :*
  type e :* l = AddField e l

  class Record e
  instance Record EmptyRecord
  instance Record l => Record (AddField e l)

  data Z = Z
  data S a = S a

  class CNum e
  instance CNum Z
  instance CNum a => CNum (S a)

  class CNum n => HasField n e l where
    (.!) :: l -> n -> e
    (.@) :: (l,n) -> e -> l

  instance HasField Z e (e :* l) where
    (AddField e _) .! Z = e
    ((AddField e l),Z) .@ e' = (AddField e' l)

  instance HasField n e l => HasField (S n) e (e' :* l) where
    (AddField _ l) .! (S n) = l .! n
    ((AddField e l),(S n)) .@ e' = (AddField e ((l,n) .@ e'))

  class CNum n => Length n r
  instance Length Z EmptyRecord
  instance Length n r => Length (S n) (AddField e r)

  infixr 8 .*
  (.*) :: e -> r -> (e :* r)
  e .* r = AddField e r

  infixl 9 .=
  (.=) :: CNum n => n -> e -> e
  _ .= e = e

  firstLabel = Z
  nextLabel = S

  firstLabel' :: forall e l . HasField Z e l => (Z, l -> e, l -> e -> l)
  firstLabel' = (Z, (\s -> s .! Z), (\s -> \x -> (s,Z) .@ x))
  nextLabel'' :: forall n' n e l . (CNum n,HasField (S n) e l) => n -> (S n, l -> e, l -> e -> l)
  nextLabel'' n = 
    let n' = S n
    in (n', (\s -> s .! n'), (\s -> \x -> (s,n') .@ x))
  nextLabel' (x,_,_) = nextLabel'' x

  {- Example
  key = firstLabel
  name = nextLabel key

  test :: Int :* String :* EmptyRecord
  test = key .= 10 
      .* name .= "Pippo"
      .* EmptyRecord

  res :: Int
  res = test .! key

  res1 :: Int :* String :* EmptyRecord
  res1 = (test,key) .@ 20 -}