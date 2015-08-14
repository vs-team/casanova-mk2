{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

module References where

  import Records

  type Getter s a = s->(a,s)
  type Setter s a = s->a->((),s)

  data Reference s a = Reference (Getter s a) (Setter s a)
  data Constant s a = Constant (Getter s a)

  infixl 7 <--

  class Value f where
    getter :: f s a -> Getter s a
    (<--) :: (CNum n, HasField n b a) => (f s a) -> n -> (f s b)
    build :: (s -> (f s a,s)) -> f s a
    from_constant :: (Constant s a) -> f s a

  instance Value Constant where
    getter (Constant getter) = getter
    (<--) (Constant getter) n = 
          (Constant (\s -> 
                        let (v,s') = getter s
                        in ((v .! n),s')))
    build f = Constant(\s ->
                        let ((Constant getter),s') = f s
                        in (getter s'))
    from_constant c = c

  instance Value Reference where
    getter (Reference getter _) = getter
    (<--) (Reference getter setter) n = 
          (Reference (\s -> 
                        let (v,s') = getter s
                        in ((v .! n),s'))
                     (\s -> \x ->
                        let (v,s') = getter s
                            v' = (v,n) .@ x
                            ((),s'') = setter s' v'
                        in ((),s'')))
    build f = (Reference (\s ->
                           let ((Reference getter _),s') = f s
                           in (getter s'))
                         (\s -> \x ->
                           let ((Reference _ setter),s') = f s
                           in (setter s' x)))
    from_constant (Constant getter) = (Reference getter (\s -> \x ->
                                                            let (_,s') = getter s
                                                            in ((),s')))

  setter (Reference _ setter) = setter

  (<==) :: (CNum n, HasField n b a) => (Reference s a) -> (n,(a->b),(a->b->a)) -> (Reference s b)
  (<==) (Reference getter setter) (_,read,write) = 
        (Reference (\s -> 
                      let (v,s') = getter s
                      in (read v,s'))
                   (\s -> \x ->
                      let (v,s') = getter s
                          v' = write v x
                          ((),s'') = setter s' v'
                      in ((),s'')))

  infixl 9 =:
  (=:) :: Reference s a -> a -> Reference s ()
  (Reference getter setter) =: x = from_constant(Constant(\s -> setter s x))

  this :: Reference s s
  this = Reference (\s -> (s,s)) (\s -> \x -> ((),x))

  {-key = firstLabel
  name = nextLabel key

  test :: Int :* String :* EmptyRecord
  test = key .= 10 
      .* name .= "Pippo"
      .* EmptyRecord
  
  v = (test <= key)
  test <= (v+1)-}