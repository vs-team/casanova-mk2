#!usr/bin/python
#pygame tetris
import sys , os , math , random
import pygame
from pygame. locals import *

WAITING_TIME=0
FPS=5
GRAY=(200,200,200)
WHITE=(255,255,255)
BLACK=(0,0,0)
START_COORDINATES=(180,50)
BLOCK_WIDTH=24
SCREEN_SIZE=[400,600]
TETRA_CHOICES=(1,4)

GRID_SIZE_X=12
GRID_SIZE_Y=20
#0 to 400

########NOTE:
########    Blocks are the 1x1 blocks, the one that makes up the grid
########    Tetras are the ones made up by 4 blocks making weird shapes
    
class Block(pygame.sprite.Sprite):
    def __init__ (self,block_type, xy):
        "just the basic building block of tetras, makes up the grid"
        pygame.sprite.Sprite.__init__(self)
        self.block_type=block_type
        if block_type==1:            
            self.image =pygame.image.load( os.path.join( 'basic_block1.gif' ))
        elif block_type==2:
            self.image =pygame.image.load( os.path.join( 'basic_block2.gif' ))
        elif block_type==3:    
            self.image =pygame.image.load( os.path.join( 'basic_block3.gif' ))
        elif block_type==4:
            self.image =pygame.image.load( os.path.join( 'basic_block4.gif' ))
        elif block_type==5:
            self.image =pygame.image.load( os.path.join( 'basic_block5.gif' ))
        self.image.convert()
        self.rect = self.image.get_rect()
        
        self.rect.left, self.rect.top = xy
    def transform(self,block_type):
        self.block_type=block_type
        if block_type==1:            
            self.image =pygame.image.load( os.path.join( 'basic_block1.gif' ))
        elif block_type==2:
            self.image =pygame.image.load( os.path.join( 'basic_block2.gif' ))
        elif block_type==3:    
            self.image =pygame.image.load( os.path.join( 'basic_block3.gif' ))
        elif block_type==4:
            self.image =pygame.image.load( os.path.join( 'basic_block4.gif' ))
        elif block_type==5:
            self.image =pygame.image.load( os.path.join( 'basic_block5.gif' ))
        self.image.convert()
            
class Game(object):
    def __init__(self):
        pygame.init()     
        self.window = pygame.display.set_mode(SCREEN_SIZE)
        
        self.clock = pygame.time.Clock()
        
        self.background = pygame.Surface(SCREEN_SIZE)
        self.background.fill(GRAY)
          
        fonts = pygame.font.get_fonts()

        self.font1 = pygame.font.Font(pygame.font.match_font("Arial"), 18)
        self.score=0
        self.text1 = self.font1.render('TETRIS Score: %s'%self.score, 1, WHITE)#,(159, 182,205))
        
        self.rect1 = self.text1.get_rect()
        
        self.rect1.centery = 20
        self.rect1.centerx = 200

        self.background.blit(self.text1, self.rect1)
        
        self.window.blit(self.background, (0,0))
        pygame.display.flip()

        self.sprites = pygame.sprite.RenderUpdates()
        self.sprites_to_update=pygame.sprite.RenderUpdates()
        
        self.block_grid=[[Block(5,(0,0)) for y in range (0,GRID_SIZE_Y,1)] for x in range (0,GRID_SIZE_X,1)]#y is the y axis
        x_coordinate=60
        for i in range (0,GRID_SIZE_X,1):
            x_coordinate+=BLOCK_WIDTH
            y_coordinate=50
            for j in range(0,GRID_SIZE_Y,1):
                #print i,j
                self.block_grid[i][j].rect.topleft=x_coordinate,y_coordinate
                self.block_grid[i][j].indexes=i,j
                self.sprites.add(self.block_grid[i][j])
                y_coordinate+=BLOCK_WIDTH
        self.current_tetra=[]#will have 4 INDEXES of blocks stored in it
        
    def handleEvents(self):        
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return False
            elif event.type==pygame.KEYDOWN and self.current_tetra:
                if event.key==pygame.K_RIGHT:
                    self.moveTetra("right")
                        
                elif event.key==pygame.K_LEFT:
                     self.moveTetra("left")

                elif event.key==pygame.K_DOWN:
                    self.moveTetra("straight down")
                elif event.key==pygame.K_UP:
                    self.rotateTetra()
                pygame.time.set_timer(pygame.KEYDOWN, WAITING_TIME)
        return True

    def removeBlocksForRotation(self):
        self.block_grid[self.current_tetra[0][0]][self.current_tetra[0][1]].transform(5)
        self.block_grid[self.current_tetra[2][0]][self.current_tetra[2][1]].transform(5)
        self.block_grid[self.current_tetra[3][0]][self.current_tetra[3][1]].transform(5)

    def transformBlocksForRotation(self,b_type):
        self.block_grid[self.current_tetra[0][0]][self.current_tetra[0][1]].transform(b_type)
        self.block_grid[self.current_tetra[2][0]][self.current_tetra[2][1]].transform(b_type)
        self.block_grid[self.current_tetra[3][0]][self.current_tetra[3][1]].transform(b_type)
        
    def areBoxesFilled(self,index1,index2,index3):
        "checks if boxes are filled AND if indexes are not negative!"
        for i in (index1,index2,index3):
            if self.block_grid[i[0]][i[1]].block_type!=5 and i not in self.current_tetra:
                print self.current_tetra
                print i
                print "ITS FILLED!"
                return True
        for i in (index1,index2,index3):
            if i[0]<0 or i[1]<0:
                return True
        return False
    
    def rotateTetra(self):
        b_type=self.block_grid[self.current_tetra[0][0]][self.current_tetra[0][1]].block_type
        if b_type!=1:
            if b_type==2:
                if self.current_tetra[0][1]+1==self.current_tetra[1][1]:#takes form of |
                    if not self.areBoxesFilled([self.current_tetra[1][0]-1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0]+1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0]+2,self.current_tetra[1][1]]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0]-1,self.current_tetra[1][1]]
                        self.current_tetra[2]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]
                        self.current_tetra[3]=[self.current_tetra[1][0]+2,self.current_tetra[1][1]]

                        self.transformBlocksForRotation(b_type)

                else :
                    if not self.areBoxesFilled([self.current_tetra[1][0],self.current_tetra[1][1]-1],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]+2]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0],self.current_tetra[1][1]-1]
                        self.current_tetra[2]=[self.current_tetra[1][0],self.current_tetra[1][1]+1]
                        self.current_tetra[3]=[self.current_tetra[1][0],self.current_tetra[1][1]+2]

                        self.transformBlocksForRotation(b_type)
            elif b_type==3:
                if self.current_tetra[0][0]+1==self.current_tetra[1][0]:#T
                    if not self.areBoxesFilled([self.current_tetra[1][0],self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]-1],
                                          [self.current_tetra[1][0]+1,self.current_tetra[1][1]]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0],self.current_tetra[1][1]+1]
                        self.current_tetra[2]=[self.current_tetra[1][0],self.current_tetra[1][1]-1]
                        self.current_tetra[3]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]

                        self.transformBlocksForRotation(b_type)
                    
                elif self.current_tetra[0][1]-1==self.current_tetra[1][1]:#|-
                    if not self.areBoxesFilled([self.current_tetra[1][0]+1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0]-1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]-1]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]
                        self.current_tetra[2]=[self.current_tetra[1][0]-1,self.current_tetra[1][1]]
                        self.current_tetra[3]=[self.current_tetra[1][0],self.current_tetra[1][1]-1]

                        self.transformBlocksForRotation(b_type)

                elif self.current_tetra[0][0]-1==self.current_tetra[1][0]:#upside down T
                    if not self.areBoxesFilled([self.current_tetra[1][0],self.current_tetra[1][1]-1],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0]-1,self.current_tetra[1][1]]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0],self.current_tetra[1][1]-1]
                        self.current_tetra[2]=[self.current_tetra[1][0],self.current_tetra[1][1]+1]
                        self.current_tetra[3]=[self.current_tetra[1][0]-1,self.current_tetra[1][1]]

                        self.transformBlocksForRotation(b_type)

                elif self.current_tetra[0][1]+1==self.current_tetra[1][1]:#-|
                    if not self.areBoxesFilled([self.current_tetra[1][0]-1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0]+1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]+1]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0]-1,self.current_tetra[1][1]]
                        self.current_tetra[2]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]
                        self.current_tetra[3]=[self.current_tetra[1][0],self.current_tetra[1][1]+1]

                        self.transformBlocksForRotation(b_type)
                    
            elif b_type==4:
                if self.current_tetra[2][1]-1==self.current_tetra[1][1]:#>^>
                    if not self.areBoxesFilled([self.current_tetra[1][0]+1,self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0]+1,self.current_tetra[1][1]],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]-1]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]+1]
                        self.current_tetra[2]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]
                        self.current_tetra[3]=[self.current_tetra[1][0],self.current_tetra[1][1]-1]

                        self.transformBlocksForRotation(b_type)
                    print "rotate"
                    
                elif self.current_tetra[2][0]-1==self.current_tetra[1][0]:#V>V
                    if not self.areBoxesFilled([self.current_tetra[1][0]-1,self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0],self.current_tetra[1][1]+1],
                                          [self.current_tetra[1][0]+1,self.current_tetra[1][1]]):
                        self.removeBlocksForRotation()

                        self.current_tetra[0]=[self.current_tetra[1][0]-1,self.current_tetra[1][1]+1]
                        self.current_tetra[2]=[self.current_tetra[1][0],self.current_tetra[1][1]+1]
                        self.current_tetra[3]=[self.current_tetra[1][0]+1,self.current_tetra[1][1]]
                    
                        self.transformBlocksForRotation(b_type)
    def displayGrid(self):
        for sprite in self.sprites_to_update:
            sprite.update()
            #print sprite.rect.topleft
        self.sprites.draw(self.background)
        self.window.blit(self.background,(0,0))
        pygame.display.flip()

    def createNewCurrentTetra(self):
        "returns True if possible, False if cannot, meaning a loss"
        current_type=random.randint(TETRA_CHOICES[0],TETRA_CHOICES[1])#1 is 2x2, 2 is 1x4, 3 is upside down T, 4 is an S
        if current_type==1:#square
            if (self.block_grid[4][0].block_type==5 and self.block_grid[5][0].block_type==5
                and self.block_grid[4][1].block_type==5 and self.block_grid[5][1].block_type==5):
                self.current_tetra.append([4,0])
                self.current_tetra.append([4,1])
                self.current_tetra.append([5,0])
                self.current_tetra.append([5,1])
            else:
                return False
                
        elif current_type==2:#line
            if (self.block_grid[4][0].block_type==5 and self.block_grid[4][2].block_type==5
                and self.block_grid[4][1].block_type==5 and self.block_grid[4][3].block_type==5):
                self.current_tetra.append([4,0])
                self.current_tetra.append([4,1])
                self.current_tetra.append([4,2])
                self.current_tetra.append([4,3])
            else:
                return False
            
        elif current_type==3:#T
            if (self.block_grid[4][0].block_type==5 and self.block_grid[5][0].block_type==5
                and self.block_grid[5][1].block_type==5 and self.block_grid[6][0].block_type==5):
                self.current_tetra.append([4,0])
                self.current_tetra.append([5,0])
                self.current_tetra.append([6,0])
                self.current_tetra.append([5,1])
            else:
                return False

        elif current_type==4:#S
            if (self.block_grid[5][0].block_type==5 and self.block_grid[6][0].block_type==5
                and self.block_grid[4][1].block_type==5 and self.block_grid[5][1].block_type==5):
                self.current_tetra.append([4,1])
                self.current_tetra.append([5,0])
                self.current_tetra.append([5,1])
                self.current_tetra.append([6,0])
            else:
                return False
        for index in self.current_tetra:
            self.block_grid[index[0]][index[1]].transform(current_type)
        return True
            
    def moveTetra(self,movement):
        temp_tetra=[]
        try:
            current_type=self.block_grid[self.current_tetra[0][0]][self.current_tetra[0][1]].block_type
        except IndexError:
            pass
        if movement=="down":        
            for index in self.current_tetra:
                if index[1]+1<=GRID_SIZE_Y-1:
                    if self.block_grid[index[0]][index[1]+1].block_type==5 or [index[0],index[1]+1] in self.current_tetra:
                        
                        temp_tetra.append([index[0],index[1]+1])                        
                    else:
                        self.current_tetra=[]
                        return False
                else:
                    self.current_tetra=[]
                    return False

        elif movement=="straight down":
            while True:
                if not self.moveTetra("down"):
                    print "YES"
                    #return False
                    break
        elif movement=="left":
            for index in self.current_tetra:
                if index[0]-1>=0:
                    if self.block_grid[index[0]-1][index[1]].block_type==5 or [index[0]-1,index[1]] in self.current_tetra:
                        
                        temp_tetra.append([index[0]-1,index[1]])
                    else:
                        return False
                else:
                    print "WHOOPS!"
                    return False
        elif movement=="right":
            for index in self.current_tetra:
                if index[0]+1<=GRID_SIZE_X-1:
                    if self.block_grid[index[0]+1][index[1]].block_type==5 or [index[0]+1,index[1]] in self.current_tetra:
                        
                        temp_tetra.append([index[0]+1,index[1]])
                            
                    else:
                        return False
                else:
                    print "WHOOPS!"
                    return False

        for index in self.current_tetra:
            self.block_grid[index[0]][index[1]].transform(5)
        for index in temp_tetra:
            self.block_grid[index[0]][index[1]].transform(current_type)
        self.current_tetra=temp_tetra+[]
        self.displayGrid()    
        return True

    def clearLines(self):
        is_line_full=False
        for y in range (GRID_SIZE_Y-1,-1,-1):
            if is_line_full:
                y+=1
            else:
                is_line_full=True
            for x in range(0,GRID_SIZE_X,1):
                if self.block_grid[x][y].block_type==5:
                    is_line_full=False
                    break
            if is_line_full:
                for z in range (y-1,-1,-1):
                    for x in range(0,GRID_SIZE_X,1):
                        self.block_grid[x][z+1].transform(self.block_grid[x][z].block_type)
                for x in range(0,GRID_SIZE_X,1):
                        self.block_grid[x][0].transform(5)
                        
                self.score+=1
                self.text1 = self.font1.render('TETRIS Score: %s'%self.score, 1, WHITE)
                self.background.fill(GRAY,self.rect1)
                self.background.blit(self.text1, self.rect1)
                self.window.blit(self.background, (0,0))
        self.displayGrid()
                
    def run(self):        
        print 'Starting Event Loop'
        running = True
        lost=False
        while running and not lost:
            self.clock.tick(FPS)                
            if not self.current_tetra:
                if not self.createNewCurrentTetra():
                    self.text1 = self.font1.render('Loser. Score: %s'%self.score, 1, WHITE)
                    self.background.fill(GRAY,self.rect1)
                    self.background.blit(self.text1, self.rect1)
                    self.window.blit(self.background, (0,0))
                    #pygame.time.wait(10000)
                    lost=True
            else:
                running=self.handleEvents()
                self.moveTetra("down")

            if not self.current_tetra:
                self.clearLines()
                
                #self.displayGrid()
            pygame.display.set_caption('Pygame - Pong')# %d fps' % self.clock.get_fps())
            
        while lost:
            lost=self.handleEvents()
        print 'Quitting. Thanks for playing'
        pygame.quit()
        sys.exit()

if __name__ == '__main__':
    game = Game()
    game.run()
