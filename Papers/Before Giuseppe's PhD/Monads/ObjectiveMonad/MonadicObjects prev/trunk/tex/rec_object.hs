{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

data Method a b t = Method (a -> (b,t))
type Counter k = (Int, Method () () k)

data RecCounter = RecCounter(Counter RecCounter)

class RecEquivalence a where
  type To a :: *
  to :: a -> To a
  from :: To a -> a

instance RecEquivalence RecCounter where
  type To RecCounter = Counter RecCounter
  to (RecCounter c) = c
  from = RecCounter
