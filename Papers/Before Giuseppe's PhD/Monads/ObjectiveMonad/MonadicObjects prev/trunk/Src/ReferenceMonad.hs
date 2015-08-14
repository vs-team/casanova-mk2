{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

module ReferenceMonad where

  import Records
  import References

  bind :: (Value r1, Value r2) => r1 s a -> (a -> r2 s b) -> r2 s b
  bind e k =
    build(\s ->
            let (v,s') = getter e s
                res = k v
            in (res,s'))

  unit :: a -> Constant s a
  unit x = Constant (\s -> (x,s))

  instance Value v => Monad (v s) where
    (>>=) = bind
    return x = from_constant (unit x)