#! /usr/bin/env python

import sys
sys.setrecursionlimit(150000)

import time

count = 0
def my_yield():
  global count
  count = count + 1
  yield

def run_co(c):
  global count
  count = 0
  t0 = time.clock()
  for r in c: ()
  t = time.clock()
  dt = t-t0
  print(dt,count,count / dt)
  return r

def parallel_(p1,p2):
  r1 = None
  r2 = None
  while(r1 == None or r2 == None):
    if(r1 == None):
      r1 = next(p1)
    if(r2 == None):
      r2 = next(p2)
    for x in my_yield() : 
      yield
  for x in my_yield() : 
    yield r1,r2

def parallel_many_aux(ps,l,u):
  if(l < u):
    for x in parallel_(ps[l],parallel_many_aux(ps,l+1,u)):
      for y in my_yield(): 
        yield x
    yield 0

def parallel_many_(ps):
  return parallel_many_aux(ps,0,len(ps))

def parallel_first_(p1,p2):
  r1 = None
  r2 = None
  while(r1 == None and r2 == None):
    if(r1 == None):
      r1 = next(p1)
    if(r2 == None):
      r2 = next(p2)
    for x in my_yield() : 
      yield
  for x in my_yield() : 
    yield r1,r2

def wait(max_dt):
  t0 = time.clock()
  t = time.clock()
  while(t - t0 < max_dt):
    for x in my_yield() : 
      yield
    #print(t - t0)
    t = time.clock()
  for x in my_yield() : 
    yield 0

def fibo_co(n):
  if(n==0):
    for x in my_yield() : 
      yield 0
  else:
    if(n==1):
      for x in my_yield() : 
        yield 1
    else:
      for x in my_yield() : 
        yield
      for n1 in fibo_co(n-1): 
        for x in my_yield() : 
          yield
      for n2 in fibo_co(n-2): 
        for x in my_yield() : 
          yield
      for x in my_yield() : 
        yield n1+n2

def log(i):
  #print("log ", i)
  for x in wait(2.0): 
    for x in my_yield() : 
      yield
  for x in log(i+1): 
    for x in my_yield() : 
      yield

def fibo_test():
  run_co(parallel_first_(fibo_co(25),log(0)))

def many_fibs(n):
  #yield return log(0);
  for i in range(0,n+1):
    yield fibo_co(i + 5)

def many_fibs_test():
  run_co(parallel_many_(list(many_fibs(15))))

#fibo_test()
#many_fibs_test()

class Entity:
  pass
class State:
  pass

def simple_mk_ship(n):
  s = Entity()
  s.Position = 100.0 - n
  return s

def add_ship(new_ship, run_ship, state):
  state.Entities.append(new_ship)
  for result in run_ship(new_ship): 
    for x in my_yield():
      yield
  state.Entities.remove(new_ship)
  for x in my_yield():
    yield result
  
def simple_run_ship(self):
  while(self.Position > 0.0):
    self.Position = self.Position - 0.1
    for x in my_yield():
      yield
  yield 0

    
def many_ships(n,state):
  if(n > 0):
    for x in parallel_(add_ship(simple_mk_ship(n), simple_run_ship, state), many_ships(n-1,state)):
      for x in my_yield():
        yield
    yield 0
  
def log_ships(state):
  while(True):
    print("there are ", len(state.Entities), " ships")
    for x in wait(2.0):
      for x in my_yield():
        yield

def state_access_test():
  state = State()
  state.Entities = []
  #test = parallel_(many_ships(200,state),log_ships(state))
  test = many_ships(200,state)
  run_co(test)

state_access_test()
