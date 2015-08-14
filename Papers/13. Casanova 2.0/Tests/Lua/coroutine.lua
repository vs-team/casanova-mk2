Vector2 = {}
Vector2.__index = Vector2
function Vector2.create(x,y)
   local acnt = {}             -- our new object
   setmetatable(acnt,Vector2)  -- make Account handle lookup
   acnt.x = x
   acnt.y = y
   return acnt
end


Ball = {}
Ball.__index = Ball
function Ball.create()
   local acnt = {}             -- our new object
   setmetatable(acnt,Ball)  -- make Account handle lookup
   acnt.p = Vector2.create(0.0,0.0)
   acnt.v = Vector2.create(0.1,0.0)

   acnt.p_rule =
	coroutine.create(function ()
		while true do
            coroutine.yield()
			acnt.p = Vector2.create(acnt.p.x + acnt.v.x, acnt.p.y + acnt.v.y)
		end
	end)

   acnt.v_rule =
	coroutine.create(function ()
		while true do
			coroutine.yield()
			if acnt.p.x > 500 then
				acnt.p = Vector2(500, acnt.p.y)
				acnt.v = Vector2(-0.1, 0.0)
			elseif acnt.p.x < -500 then
				acnt.p = Vector2(-500, acnt.p.y)
				acnt.v = Vector2(0.1, 0.0)
			end
		end
	end)

   return acnt
end



balls = {}


for var=0,10000 do
	table.insert(balls, Ball.create())
end
function update(_index)
	coroutine.resume(balls[_index].p_rule)
	coroutine.resume(balls[_index].v_rule)
end

local now = os.clock()
for var=0,10000 do
	table.foreach(balls, update)
end
local took = os.clock()-now

print(took)
