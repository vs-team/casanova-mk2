{-# LANGUAGE MultiParamTypeClasses, FunctionalDependencies, FlexibleInstances,
  UndecidableInstances, FlexibleContexts, EmptyDataDecls, ScopedTypeVariables,
  TypeOperators, TypeSynonymInstances #-}

{-
import Records
import References
import ReferenceMonad
import Methods

----------------------------------------------
-- costruiamo il record ricorsivo per shape --
----------------------------------------------
x = firstLabel
y = nextLabel x
move = nextLabel y
draw = nextLabel move

instance RecursiveContainer RecShape (Shape RecShape) where
  open (RecShape r) = r
  pack = RecShape

type Position = (Integer,Integer)
type Shape k = Integer :* Integer :* (Method k Position ()) :* Method k () String :* EmptyRecord
data RecShape = RecShape (Shape RecShape)
mk_shape :: Position -> RecShape
mk_shape (xv,yv) = 
  RecShape(    x .= xv
            .* y .= yv
            .* move .= move_shape
            .* draw .= draw_shape
            .* EmptyRecord)


move_shape :: Method RecShape Position ()
move_shape = mk_method (\(xv,yv) ->
                 (do (this <-- x) =: (xv) :: Reference (Shape RecShape) ()
                     (this <-- y) =: (yv) :: Reference (Shape RecShape) ()
                     return ()))

draw_shape :: Method RecShape () String
draw_shape = mk_method (\() ->
                 (do xv <- (this <-- x) :: Reference (Shape RecShape) Integer
                     yv <- (this <-- y) :: Reference (Shape RecShape) Integer
                     return ("Shape at " ++ show xv ++ ", " ++ show yv)))


instance RecursiveContainer RecCircle (Circle RecCircle) where
  open (RecCircle r) = r
  pack = RecCircle

radius = firstLabel
shape_of_circle = nextLabel radius

type Circle k = Integer :* Shape RecShape :* EmptyRecord
data RecCircle = RecCircle (Circle RecCircle)
mk_circle :: Position -> Integer -> RecCircle
mk_circle p@(xv,yv) r =
  RecCircle(    radius .= r
             .* shape_of_circle .= override_draw_circle(open (mk_shape p))
             .* EmptyRecord)

override_draw_circle :: Shape RecShape -> Shape RecShape
override_draw_circle shape =
  (shape,draw) .@ draw_circle

draw_circle :: Method RecShape () String
draw_circle = mk_method (\() ->
                 (do xv <- (this <-- x) :: Reference (Shape RecShape) Integer
                     yv <- (this <-- y) :: Reference (Shape RecShape) Integer
                     return ("Circle at " ++ show xv ++ ", " ++ show yv)))

shape :: Shape RecShape
shape = open (mk_shape (10,10))

test :: Position -> Reference (Shape RecShape) String
test = ((this :: Reference (Shape RecShape) (Shape RecShape),RecShape) <<- move) :: Position -> Reference (Shape RecShape) String

test :: Reference (Shape RecShape) String
test = do (this, RecShape) <<- move (0,0) :: Reference (Shape RecShape) ()
          xv <- (this <-- x) :: Reference (Shape RecShape) Integer
          yv <- (this <-- y) :: Reference (Shape RecShape) Integer
          return (show xv ++ ", " ++ show yv)
-}

{-
----------------------------------------------------------------------------------
-- estraiamo una lista di references a una singola shape a partire da uno state --
-- questo é un esempio rognoso, ossia quello del partire da una serie di valori --
-- di tipi che ereditano una stessa classe base, e ottenere una lista mutabile  --
-- di valori della classe base: con il sistema di references ce la facciamo, e  --
-- questo é significativo perché implica che il nostro sistema é piú potente di --
-- tutti quelli che rappresentano l'ereditarietá tramite vari trucchetti        --
----------------------------------------------------------------------------------
shapes :: [Reference (State RecState) (Shape RecShape)]
shapes = 
    let (c :: Reference (State RecState) (Circle RecCircle)) = this <-- circle 
        cs = c <-- shape_of_circle
        (r :: Reference (State RecState) (Rect RecRect)) = this <-- rect
        rs = r <-- shape_of_rect
    in [rs,cs]


-------------------------------------------------------------------------------
-- invochiamo la funzione draw (astratta) di una lista di references a shape --
-- la figata é che se anche invocassimo un metodo che cambia la shape, il    --
-- cambiamento andrebbe anche nella shape originale su cui abbiamo fatto il  --
-- cast...                                                                   --
-------------------------------------------------------------------------------
draw_many :: [Reference (State RecState) (Shape RecShape)] -> String -> Reference (State RecState) String
draw_many [] acc = return acc
draw_many (s : ss) acc = 
  do s <- (((s,shape_flatten) <<- draw) .!! ()) :: Reference (State RecState) String
     draw_many ss (s ++ acc)


draw_all_shapes = fst(getter (draw_many shapes "") state0) -- "(circle at 50, 50)(rect at 0, 0)"
-}