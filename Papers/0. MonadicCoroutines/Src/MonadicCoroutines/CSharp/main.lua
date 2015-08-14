count = 0
function yield()
	count = count + 1
	coroutine.yield()
end

function run_co(c) 
  count = 0
  local t0 = os.time()
	local s,r = true,nil
	while(s) do
		r_old = r
  	s,r = coroutine.resume(c)
  end
  local t = os.time()
  local dt = t-t0
  print(dt,count,count / dt)
  return r_old
end

function bind_co(c) 
	local s,r,r_old = true,nil,nil
	while(s) do
		r_old = r
	  	s,r = coroutine.resume(c)
	  	yield()
    end
    return r_old
end

function parallel_(p1,p2)
	return coroutine.create(
		function()
			local s1,r1,r1_old,s2,r2,r2_old = true,nil,nil,true,nil,nil
			while(s1 or s2) do
				if(s1) then
					r1_old = r1
					s1,r1 = coroutine.resume(p1)
				end
				if(s2) then
					r2_old = r2
					s2,r2 = coroutine.resume(p2)
				end
        yield()
			end
			return r1_old,r2_old
		end)
end

function parallel_first_(p1,p2)
  return coroutine.create(
    function()
      local s1,r1,r1_old,s2,r2,r2_old = true,nil,nil,true,nil,nil
      while(s1 and s2) do
        if(s1) then
          r1_old = r1
          s1,r1 = coroutine.resume(p1)
        end
        if(s2) then
          r2_old = r2
          s2,r2 = coroutine.resume(p2)
        end
        yield()
      end
      return r1_old,r2_old
    end)
end

function parallel_many_(ps)
  return coroutine.create(
    function()
      if(ps == nil) then return 0 end
      bind_co(parallel_(ps.head,parallel_many_(ps.tail)))
      return 0
    end)
end

function wait(max_dt)
	return coroutine.create(
		function()
			local t0 = os.clock()
			local t = os.clock()
			while(t - t0 < max_dt) do
				yield()
				--print(t,t0)
				t = os.clock()
			end
			return 0
		end)
end

function fibo_co(n)
	return coroutine.create(
			function()
				if(n==0) then 
					yield()
					return 0;
				end
				if(n==1) then 
					yield()
					return 1;
				end
				yield()
				local n1 = bind_co(fibo_co(n-1))
				local n2 = bind_co(fibo_co(n-2))
				yield()
				return n1+n2
			end)
end

function log(i)
	return
		coroutine.create(
			function()
				--print("log " .. i)
        bind_co(wait(2.0))
				bind_co(log(i+1))
			end)
end

function fibo_test()
  run_co(parallel_first_(fibo_co(25),log(0)))
end

function many_fibo_test()
  local fibos = nil
  for i=0,15 do
    fibos = {head = fibo_co(i+5), tail = fibos}
  end
  run_co(parallel_many_(fibos))
end



function add_ship(mk_ship, run_ship, state)
    return coroutine.create(
      function()
        local new_ship = mk_ship()
        state.Entities = {head = new_ship, tail = state.Entities}
        local result = bind_co(run_ship(new_ship))
        local new_entities = nil
        while(state.Entities ~= nil) do
          if(state.Entities.head ~= new_ship) then
            new_entities = {head = state.Entities.head; tail = new_entities}
          end
          state.Entities = state.Entities.tail
        end
        state.Entities = new_entities
        return result
      end)
end

function simple_ship(self)
  return coroutine.create(
    function()
      while(self.Position > 0.0) do
        self.Position = self.Position - 0.01
        yield()
      end
      return 0
    end)
end

function many_ships(n,state) 
  return coroutine.create(
    function()
      if n > 0 then
        bind_co(parallel_(add_ship((function() return {Position = 100.0 - n} end), simple_ship, state), many_ships(n-1,state)))
        return 0
      else
        return 0
      end
    end)
end

function log_ships(state)
  return coroutine.create(
    function()
      while(true) do
        local n,ships = 0,state.Entities
        while(ships ~= nil) do
          n = n + 1
          ships = ships.tail
        end
        --print("there are " .. n .. " ships")
        bind_co(wait(2.0))
      end
    end)
end

function state_access_test()
  local state = {Entities = nil}
  --local test = parallel_(many_ships(200,state),log_ships(state))
  local test = many_ships(200,state)
  run_co(test)
end
  
state_access_test()
