In this library we build a monadic system for representing mutable objects in 
Haskell. A quick overview: objects are represented not with standard Haskell
records, but rather with a custom implementation based on lists of types. A
record in our system is defined first by creating its labels (defined in
Records.hs):

key = firstLabel
name = nextLabel key

then by defining its type (:* is the type list cons):

type TestType = Integer :* String :* EmptyRecord

and finally by binding values to each label:

test :: TestType
test = key .= 10 
    .* name .= "Pippo"
    .* EmptyRecord


We do not manipulate records directly, that is a function that reads and
changes values on the record we have just seen does not access the "test"
value directly; this is to be expected, because given the "test" value there
is no way in a pure language such as Haskell that we could mutate its value.
Rather than act on the value directly, we write a method that accesses a
special value, called "this". On "this" we get two operations (defined in
References.hs):

* the selection operator (<--), akin to the (.) operator of mainstream OO
    languages
* the assignment operator (=:), which "assigns" a new value to a label

Thanks to an appropriate monad (see ReferenceMonad.hs) we can bind together
selections (which can of course be nested) and assignments, resulting in
programs that look very close to corresponding OO programs in a mainstream
imperative OO language such as Java (C# :D):

res :: Reference TestType Integer
res = do v <- this <-- key
         return (v+1)

res1 :: Reference TestType ()
res1 = do v <- this <-- key
          this <-- key =: (v+1)
          return ()

The results of the two snippets seen above are (as expected):

x = getter res test -- x = (11, test)
y = getter res1 test -- y = ((), {test with key = test.key+1})

An unpleasant note (quite acceptable, though) is that the type inference
implemented in GHCI is unable to correctly sort this kind of programs.
Because of this, we need to write the "res1" value with various hints to 
the type inference system:

res1 :: Reference TestType ()
res1 = do v <- ((this <-- key)::Reference TestType Integer)
          (((this <-- key)::Reference TestType Integer) =: (v+1))::Reference TestType ()         
          return ()
