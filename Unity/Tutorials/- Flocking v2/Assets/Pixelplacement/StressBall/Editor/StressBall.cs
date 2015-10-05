// Copyright (c) 2011 Bob Berkebile
// Please direct any bugs/comments/suggestions to http://www.pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class StressBall : EditorWindow
{
	static float prevTime;
	static float deltaTime;
	static Texture2D ball;
	static Texture2D heldBall;
	static Texture2D ballVisual;
	static Texture2D shadow;
	static Texture2D background;
	static Texture2D particleVisual;
	static Vector2 velocityScale = new Vector2(.09f,.09f);
	static float gravity = .012f;
	static float friction = .6f;
	static float restitution = .71f;
	static Rect ballPosition;
	static bool dragging;
	static Vector2 ballVelocity;
	static Rect bounds = new Rect(0,0,100,100);
	static bool initialized;
	static bool render=true;
	static List<StressBall.StressBallParticle> aliveParticles = new List<StressBall.StressBallParticle>();
	static List<int> deadParticles = new List<int>();
	static double skinResetLength = .03;
	static double skinResetTime;
			
	[MenuItem("Pixelplacement/StressBall")]
	static void Init(){
		GetWindow(typeof(StressBall), false, "StressBall");
	}
	
	static void PlayModeChanged(){
		if(EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode){
			render=false;
		}else{
			render=true;
		}
	}
	
	void OnEnable(){
		if(!initialized && render){
			EditorApplication.playmodeStateChanged+=PlayModeChanged;
			initialized=true;
			ball = (Texture2D)Resources.Load("stressBall_ball");
			heldBall = (Texture2D)Resources.Load("stressBall_ball_held");
			shadow = (Texture2D)Resources.Load("stressBall_shadow");
			background = (Texture2D)Resources.Load("stressBall_background");
			particleVisual = (Texture2D)Resources.Load("stressBall_particle");
			prevTime = (float)EditorApplication.timeSinceStartup;
			ballPosition = new Rect(0,0,ball.width,ball.height);
			ballVelocity = new Vector2(2,2);
			ballVisual = ball;
		}
	}
	
	void OnGUI(){		
		//properties:
		bounds = position;
		CalculateDeltaTime();
		Event e = Event.current;
		
		//dragging:
		if (e.isMouse) {
		
			//check for drag:
			if(e.type == EventType.mouseDown && ballPosition.Contains(e.mousePosition)){
				dragging = true;
				ballVisual = heldBall;
			}
			if(e.type == EventType.mouseUp && dragging || e.mousePosition.x < 0 || e.mousePosition.x > bounds.width || e.mousePosition.y < 0 || e.mousePosition.y > bounds.height){
				dragging = false;
				ballVisual = ball;
			}
			
			//apply drag:
			if(dragging){
				ballPosition.x = ballPosition.x + e.delta.x;
				ballPosition.y = ballPosition.y + e.delta.y;
			}
			
			//constrain drag:
			if(ballPosition.x < 0){
				ballPosition.x = 0;
			}
			if(ballPosition.x > bounds.width - ballPosition.width){
				ballPosition.x = bounds.width - ballPosition.width;
			}
			if(ballPosition.y < 0){
				ballPosition.y = 0;
			}
			if(ballPosition.y > bounds.height - ballPosition.height){
				ballPosition.y = bounds.height - ballPosition.height;
			}
			
			//set ball's dragged velocity
			if(dragging){
				ballVelocity = Vector2.Scale(e.delta, velocityScale);
			}
		}
		
		//physics:
		if(!dragging){
			ballVelocity.y += gravity;
			ballPosition.x += ballVelocity.x * deltaTime;
			ballPosition.y += ballVelocity.y * deltaTime;
			
			//floor bounce:
			if(ballPosition.y + ball.height > bounds.height){
				ballPosition.y = bounds.height - ball.height;
				ballVelocity.x *= friction;
				ballVelocity.y *= -restitution;
				if(Mathf.Abs(ballVelocity.y) > .8){
					ParticleBurst(0,new Vector2(ballPosition.x+(ballVisual.width/2),ballPosition.y+ballVisual.height-3));
				}
			}
			
			//right wall bounce:
			if(ballPosition.x + ball.width > bounds.width){
				ballPosition.x = bounds.width - ball.width;
				ballVelocity.x *= -restitution;
				if(Mathf.Abs(ballVelocity.x) > .8){
					ParticleBurst(-1,new Vector2(ballPosition.x+ballVisual.width,ballPosition.y+(ballVisual.height/2)));
				}
			}
			
			//left wall bounce:
			if(ballPosition.x < 0){
				ballPosition.x = 0;
				ballVelocity.x *= -restitution;
				if(Mathf.Abs(ballVelocity.x) > .8){
					ParticleBurst(1,new Vector2(ballPosition.x,ballPosition.y));
				}
			}
		}
		
		//don't incur draw calls while the editor is rendering to avoiding affecting game performance:
		if (render) {			
			//draw visuals:
			
			//reset ball's visuals after a delay if an impact changed it:
			if(EditorApplication.timeSinceStartup - skinResetTime > skinResetLength){
				ballVisual = ball;
			}
			
			//animate all living particles:
			if(aliveParticles.Count > 0){
				foreach (StressBallParticle particle in aliveParticles) {
					particle.Animate();
				}
			}
			
			//burn all the dead particles and empty the dead particle roster:
			if(aliveParticles.Count > 0){
				foreach (int id in deadParticles) {
					//buggy solution for ensuring removal doesn't error:
					if(id < aliveParticles.Count){
						aliveParticles.RemoveAt(id);
					}
					
				}
				deadParticles = new List<int>();
			}
			
			//only draw the shadow and background art if we are below a resonable threshold (save cycles for "work" not "play" ;)
			if(bounds.width <550 && bounds.height<550){
				GUI.DrawTexture(new Rect(0,0,bounds.width,bounds.height),background);
				GUI.DrawTexture(new Rect(ballPosition.x+8,ballPosition.y+11,ballPosition.width,ballPosition.height),shadow);
			}
			
			//only paint the ball if it's within view:
			if(ballPosition.y>-ballPosition.height){
				GUI.DrawTexture(ballPosition,ballVisual);	
			}
		}
				
		Repaint();			
	}
		
	void CalculateDeltaTime(){
		float now = (float)EditorApplication.timeSinceStartup;
		deltaTime = (now - prevTime)*800;
		prevTime = now;
	}
	
	void ParticleBurst(int direction, Vector2 position){
		if(ballPosition.y>-ballPosition.height){
			ballVisual = heldBall;
			skinResetTime = EditorApplication.timeSinceStartup;
			int numberOfParticles = UnityEngine.Random.Range(5,15);
			int newDirection;
			for (int i = 0; i < numberOfParticles; i++) {
				//if floor particles are needed generate random direction:
				if(direction==0){
					int seed = UnityEngine.Random.Range(0,2);
					if(seed==0){
						newDirection=-1;
					}else{
						newDirection=1;	
					}
				}else{
					newDirection=direction;
				}
				
				aliveParticles.Add(new StressBallParticle(newDirection,position));
			}	
		}
		
	}
	
	class StressBallParticle{
		Vector2 position;
		Vector2 velocity;
		Vector2 damping = new Vector2(.999f,.999f);
		float decay;
		float bounce = -.6f;
		float gravity = .0095f;
		float alpha = 1.2f;
		int direction;
		
		public StressBallParticle(int direction, Vector2 position){
			decay = UnityEngine.Random.Range(.0015f,.005f);
			this.position=position;
			velocity=new Vector2(UnityEngine.Random.Range(.05f,.6f)*direction,UnityEngine.Random.Range(-.8f,-1.4f));
		}
		
		public void Animate(){
			//apply alpha fade:
			GUI.color = new Color(1,1,1, alpha);
			
			//calculate and apply "physics":
			velocity = Vector2.Scale(velocity, damping);
			velocity.y+=gravity;
			position+=velocity;
			
			//bounce off floor:
			if(position.y>StressBall.bounds.height-particleVisual.height){
				position.y=StressBall.bounds.height-particleVisual.height;
				velocity.y*=bounce;
			}
			
			//draw particle:
			GUI.DrawTexture(new Rect(position.x,position.y,particleVisual.width,particleVisual.height), particleVisual);			
			alpha -= decay;
			if (alpha<=0) {
				//get the id of this particle in the living particle list and inject it in the dead particle list:
				StressBall.deadParticles.Add(StressBall.aliveParticles.IndexOf(this));
			}
			GUI.color = Color.white;
		}
	}	
}
