import time
#out_file = open("test.txt","w")

class Vector2(object):
    x = 0.0
    y = 0.0
    def __init__(self, x1, y1):
        self.x = x1
        self.y = y1

class Ball(object):
    p = Vector2(0.0,0.0)
    v = Vector2(0.1,0.0)
	
    def p_rule(self):
        while True:
            yield 0
            self.p = Vector2(self.p.x + self.v.x, self.p.y + self.v.y)
    def v_rule(self):
        while True:
            yield 0
            if(self.p.x > 500):
                self.p = Vector2(500, self.p.y)			
                self.v = Vector2(-0.1,0.0)
            if(self.p.x < -500):
                self.p = Vector2(-500, self.p.y)
                self.v = Vector2(0.1,0.0)

class BallIterator:
    ball = Ball()
    p_iterator = ball.p_rule()
    v_iterator = ball.v_rule()
			
#rule p =
#   yield p + v * dt
#rule v =
#  wait p.X < 500
#  yield -v
#  wait p.X > 0
#  yield -v


balls = []

for i in range(10000):
    balls.append(BallIterator())

start = time.time()
for i in range(10000):
    for b in balls:       
        next(b.p_iterator)
        next(b.v_iterator)
    #print ('iteration ' + (str(i)) + '\n')
end  = time.time()
print (str (end - start)+'\n')
#out_file.write(str (end - start)+'\n')

#out_file.close()