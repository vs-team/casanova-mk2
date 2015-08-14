{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators #-}

module HTest where

  import HList
  
  data Breed = Cow | Sheep    deriving (Eq,Show,Read)
  data Disease = BSE | FM

  key = firstLabel
  name = nextLabel key
  breed = nextLabel name
  price = nextLabel breed
  disease = nextLabel price

  angus = key .=. 42
       .*. name .=. "angus"
       .*. breed .=. Cow
       .*. price .=. 75.5
       .*. HNil

  test :: Field () (Integer :*: [Char] :*: Breed :*: Double :*: HNil) = 
    (this <<= price) >>== \v ->
    (this <<= price) .= (v+10.0)
    
  res = (field_get test) angus -- res = {42, "angus", Cow, 85.5}


  v = firstLabel
  f = nextLabel v

  data MyObject = MyObject (Integer :*: (()->((),MyObject)) :*: HNil)

  {-mk_myobject cnt =  
                  MyObject(
                               v .=. cnt
                           .*. f .=. (\() -> ((),mk_myobject (cnt+1)))
                           .*. HNil)

  test' :: ReadonlyField Integer (Integer :*: (()->((),MyObject)) :*: HNil) = 
    (this <<= v) >>=== \x ->
    ret x-}

