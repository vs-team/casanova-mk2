-- esempio1: ereditarietá (tra Point3D e Point2D); il codice é molto generale
-- in quanto non implica nulla circa il tipo di esecuzione

length2d : Label Point2D (Method Point2D Unit Float)
length3d : Label Point3D (Method Point3D Unit Float)
normalize : Label Point3D (Method Point3D Unit Unit)

f : Heap h ref st, Point3D <: Point2D => ref h Point 3D -> st h float
f p = 
  do  l <- (p <= length3d) ()
      (p <= normalize) ()
      l' <- (p <= length2d) ()
      return l / l'


-- esempio2: transazioni; il predicato Transactional ci assicura la presenza
    degli statement beginT, commitT e abortT
banker : Heap h ref st, Transactional st => [Int] -> [Int] -> ref h Account
banker ws ds account = 
  do  begintT
      transactions ws ds account
  where transactions [] [] =
          do  b <- (account <= get_balance) ()
              if b < 0 then abortT
              else commitT
        transactions (w:ws) [] = 
          do  (account <= withdraw) w
              transactions ws []
        transactions ws (d:ds) = 
          do  (account <= deposit) d
              transactions ws ds


-- esempio3: canali di comunicazione; il predicato Concurrent ci assicura la presenza
    degli operatori concorrenti fork e sleep
main : Heap h ref st, Concurrent st => () -> st h ()
main() = 
  do+ channel <- new Int
      fork (p1 channel 0)
           (p2 channel)
  where p1 channel i =
          do  channel := i -- send
              sleep 1000
              i' <- eval channel -- receive
              p1 channel (i'+1)
        p2 channel =
          do  i <- eval channel -- receive
              channel := (i*2)
              p2 channel


-- esempio4: reactive programming; il predicato Reactive ci garantisce la presenza degli
    operatori di assegnamento e somma reattivi (!= e !+); ci aspettiamo che questo codice
    ritorni (5,0)
f : Num a, Heap h ref st, Reactive ref st => ref h a -> ref h a -> ref h a -> st h a
f a b c =
  do  b := 10
      c := -5
      a != b !+ c
      x <- a
      b := 5
      x' <- a
      return (x,x')