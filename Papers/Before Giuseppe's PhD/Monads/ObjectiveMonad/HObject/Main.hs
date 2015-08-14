{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances, TypeFamilies #-}

--module Main where

import Records
import References
import ReferenceMonad
import Methods

{-
key = firstLabel
name = nextLabel key

type TestType = Integer :* String :* EmptyRecord
test :: TestType
test = key .= 10 
    .* name .= "Pippo"
    .* EmptyRecord

res :: Reference TestType Integer
res = do v <- this <-- key
         return (v+1)

res1 :: Reference TestType ()
res1 = do v <- ((this <-- key)::Reference TestType Integer)
          (((this <-- key)::Reference TestType Integer) =: (v+1))::Reference TestType ()         
          return ()

x = getter res test -- x = (11, test)
y = getter res1 test -- y = ((), {test with key = test.key+1})
-}

{-
dichiariamo le labels del nostro record
-}
val = firstLabel
incr = nextLabel val

val' :: (Z,((Counter RecCounter) -> Integer), (Counter RecCounter) -> Integer -> (Counter RecCounter))
val' = firstLabel'
incr' :: (S Z,((Counter RecCounter) -> Method RecCounter () ()), (Counter RecCounter) -> Method RecCounter () () -> (Counter RecCounter))
incr' = nextLabel' val'

{-
dichiariamo due tipi per il nostro record:
serve perché altrimenti con i metodi otteniamo
un tipo infinito
-}
type Counter k = (Integer :* Method k () () :* EmptyRecord)
data RecCounter = RecCounter (Counter RecCounter)

{-
mettiamo in relazione i due tipi che definiscono il
record, in modo che il sistema di oggetti possa
autonomamente convertire istanze ricorsive (di RecCounter)
in istanze "appiattite" (di Counter RecCounter)
-}
instance Recursive (Counter RecCounter) where
  {- 
  le type functions sono una feature molto nuova
  di Haskell (ce l'ha mostrate SPJ a Oxford) che
  permette di far funzionare i metodi: senza
  queste il type checker non é in grado di
  risolvere alcuni predicati di tipo perché non
  fa SDL-risoluzione (che sarebbe l'unico modo)
  -}
  type Rec (Counter RecCounter) = RecCounter
  cons = RecCounter
  elim (RecCounter r) = r

{-
costruiamo un metodo, che ha tipo () -> Reference RecCounter ();
prima costruiamo il metodo originale, che ha tipo
() -> Reference (Counter RecCounter) (), e poi lo convertiamo
con mk_method alla sua forma finale
-}
m :: Method RecCounter () ()
m = mk_method (\() ->
                 (do v <- ((this <-- val) :: Reference (Counter RecCounter) Integer)
                     (this <-- val) =: (v+1) :: Reference (Counter RecCounter) ()
                     return ()))

x |> f = f x

{-
costruiamo il nostro record iniziale
-}
test' :: Counter RecCounter
test' = RecCounter(    val .= 0
                    .* incr .= m
                    .* EmptyRecord) |> elim

{-
invochiamo due volte il metodo incr e poi
leggiamo il contatore
-}
res2 :: Reference (Counter RecCounter) Integer
res2 = do (this <<- incr) () :: Reference (Counter RecCounter) ()
          (this <<- incr) () :: Reference (Counter RecCounter) ()
          v <- (this <-- val)
          return v

res3 = do v <- (this <== val')
          (this <== val') =: (v+1)
          v <- (this <== val')
          return v

--count é pari a 2: YAY!
count = fst (getter res2 test')
