{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators #-}

module HList where
  data HNil = HNil deriving (Eq,Show,Read)
  data HCons e l = HCons e l deriving (Eq,Show,Read)
  (.*.) :: HList l => e -> l -> HCons e l
  e .*. l = HCons e l
  infixr 7 .*.

  infixr 7 :*:
  type e :*: l = HCons e l

  class HList l
  instance HList HNil
  instance HList l => HList (HCons e l)

  class HOccurs e l
    where hOccurs :: l -> e
  instance HOccurs e (HCons e l)
    where hOccurs (HCons e _) = e
  instance HOccurs e l => HOccurs e (HCons e' l)
    where hOccurs (HCons _ l) = hOccurs l

  data HZero = HZero
  data HSucc x = HSucc x

  firstLabel = HZero
  nextLabel x = HSucc x

  class HNat n
  instance HNat HZero
  instance HNat x => HNat (HSucc x)

  class HNat n => HLookupByHNat n l e | n l -> e
    where hLookupByHNat :: n -> l -> e
    
  instance HLookupByHNat HZero (HCons e l) e
    where hLookupByHNat n (HCons e l) = e

  instance HLookupByHNat n l e => HLookupByHNat (HSucc n) (HCons e' l) e
    where hLookupByHNat (HSucc n) (HCons _ l) = hLookupByHNat n l

  class HNat n => HUpdateAtHNat n e l l' | n e l -> l'
    where hUpdateAtHNat :: n -> e -> l -> l'
    
  instance HUpdateAtHNat HZero e (HCons e l) (HCons e l)
    where hUpdateAtHNat n e (HCons _ l) = (HCons e l)

  instance HUpdateAtHNat n e l l' => HUpdateAtHNat (HSucc n) e (HCons e' l) (HCons e' l')
    where hUpdateAtHNat (HSucc n) e (HCons e' l) = (HCons e' (hUpdateAtHNat n e l))
    
  infixr 9 |>
  x |> f = f x

  infixr 8 .=.
  (.=.) :: HNat n => n -> e -> e
  n .=. e = e

  newtype Field a s = Field ((s -> (a,s)), (s -> a -> ((),s)))
  field_get (Field(get,set)) = get
  field_set (Field(get,set)) = set
  newtype ReadonlyField a s = ReadonlyField (s -> (a,s))
  readonlyfield_get (ReadonlyField(get)) = get

  this :: Field s s
  this = Field((\s -> (s,s)),(\s -> \x -> ((),x)))

  (<<=) :: (HNat n, HLookupByHNat n a b, HUpdateAtHNat n b a a) => Field a s -> n -> Field b s
  self <<= n = 
    let get s =
          let (v,s') = (field_get self) s
          in (v |> hLookupByHNat n, s')
        set s x =
          let (v,s') = (field_get self) s
              v' = v |> hUpdateAtHNat n x
          in (field_set self) s' v'
    in Field(get,set)
   
  infixr 8 .=
  (.=) :: Field a s -> a -> Field () s
  self .= v =
    let get s = (field_set self) s v
        set s () = get s
    in Field(get,set)   

  (>>==) :: Field a s -> (a -> Field b s) -> Field b s
  curr >>== step =
    let get s = 
          let (v,s') = (field_get curr) s
          in (field_get (step v)) s'
        set s y =
          let (v,s') = (field_get curr) s
          in (field_set (step v)) s' y
    in Field(get,set)

  (>>===) :: Field a s -> (a -> ReadonlyField b s) -> ReadonlyField b s
  curr >>=== step =
    let get s = 
          let (v,s') = (field_get curr) s
          in (readonlyfield_get (step v)) s'
    in ReadonlyField(get)

  (>>>==) :: ReadonlyField a s -> (a -> ReadonlyField b s) -> ReadonlyField b s
  curr >>>== step =
    let get s = 
          let (v,s') = (readonlyfield_get curr) s
          in (readonlyfield_get (step v)) s'
    in ReadonlyField(get)

  (>>>===) :: ReadonlyField a s -> (a -> Field b s) -> Field b s
  curr >>>=== step =
    let get s = 
          let (v,s') = (readonlyfield_get curr) s
          in (field_get (step v)) s'
        set s y =
          let (v,s') = (readonlyfield_get curr) s
          in (field_set (step v)) s' y
    in Field(get,set)

  ret :: a -> ReadonlyField a s
  ret x = ReadonlyField(\s -> (x,s))

  newtype Method a b s = Method (s -> a -> (b,s))
  method_call :: Method a b s -> s -> a -> (b,s)
  method_call (Method(m)) s a = m s a

  (<<=!) :: (HNat n, HLookupByHNat n a (a->b->(c,a))) => Field a s -> n -> Method b c s
  self <<=! n = 
    let m s x =
          let (v,s') = (field_get self) s
              m' = v |> hLookupByHNat n
              (y,v') = m' v x
              ((),s'') = (field_set self) s' v'
          in (y,s'')
    in Method(m)
   
  infixr 8 .!
  (.!) :: Method a b s -> a -> ReadonlyField b s
  self .! v =
    let get s = method_call self s v
    in ReadonlyField(get)

