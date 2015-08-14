{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

module Methods where

  import Records
  import References

  {-
  un metodo prende un a e ritorna una reference di b
  -}
  type Method s a b = (a -> Reference s b)
  
  {-
  un record dotato di metodi va definito come ricorsivo
  affinché non risulti in un tipo infinito; infatti un
  record rischierebbe di aver tipo:
  type MyObject = ... .* Method MyObject a b .* ...
  che non é accettabile; per questo definiamo:
  type MyObject k = ... .* Method k a b .* ...
  dove k verrá istanziato ad un opportuno contenitore:
  data RecMyObject = RecMyObject (MyObject RecMyObject)
  -}
  class Recursive s where
    type Rec s :: *
    cons :: s -> Rec s
    elim :: Rec s -> s

  
  {-
  in virtú del fatto che i metodi ritornano il contenitore
  ricorsivo del record, e non il record stesso, definiamo
  una funzione che prende il metodo definito in modo
  naturale (ossia in termini del record vero e proprio)
  e lo converte alla forma che ci serve
  -}
  mk_method :: forall a b s . (Recursive s) => Method s a b -> Method (Rec s) a b
  mk_method m =
        \(x :: a) ->
            let (Reference getter setter) = m x
            in
                (Reference (\(rs :: Rec s) -> 
                               let rs' = elim rs :: s
                                   (v,rs'') = getter rs' :: (b,s)
                               in (v,cons rs'') :: (b,Rec s))
                            (\(rs :: Rec s) -> \(x :: b) ->
                               let rs' = elim rs :: s
                                   (v,rs'') = getter rs' :: (b,s)
                                   ((),rs''') = setter rs'' x :: ((),s)
                               in ((),cons rs''')))


  {-
  diamo un operatore di selezione per i metodi
  -}
  (<<-) :: forall a s b c n . (CNum n, Recursive a, HasField n (Method (Rec a) b c) a) => (Reference s a) -> n -> (b -> Reference s c)
  (<<-) (Reference get set) n =
    \(x :: b) -> 
        from_constant( Constant(\(s :: s)->
                          let (v,s') = get s :: (a,s)
                              m = v .! n
                              ry = m x :: Reference (Rec a) c
                              (y,v') = getter ry (cons v) :: (c,Rec a)
                              v'' = elim v'
                              ((),s'') = set s' v''
                              in (y,s'')))
