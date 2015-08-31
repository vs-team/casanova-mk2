using UnityEngine;

public class UnityAnimal : MonoBehaviour 
{
	
	//vars for the whole sheet
	public int colCount =  4;
	
	
	//SetSpriteAnimation
	void SetSpriteAnimation()
	{
		// An atlas is a single texture containing several smaller textures.
		// It's used for GUI to have not power of two textures and gain space, for example.
		// Here, we have an atlas with 16 faces
		// Calculate index

		
		// Size of every cell
		float sizeX = 1.0f / colCount; // We split the texture in 4 rows and 4 cols
    float sizeY = 1.0f;
		Vector2 size =  new Vector2(sizeX,sizeY);
		
		// split into horizontal and vertical index
		var uIndex = index % colCount;
		var vIndex = index / colCount;
		
		// build offset
		// v coordinate is the bottom of the image in opengl so we need to invert.
    float offsetX = (uIndex + colCount) * size.x;
		float offsetY = (1.0f - size.y) - (vIndex + 1) * size.y;
		Vector2 offset = new Vector2(offsetX,offsetY);
		
		// We give the change to the material
		// This has the same effect as changing the offset value of the material in the editor.
		renderer.material.SetTextureOffset ("_MainTex", offset); // Which face should be displayed
		renderer.material.SetTextureScale  ("_MainTex", size); // The size of a single face
	}

  public int AnimationNumber
  {
    set
    {
      colCount = value;
    }
    get { return colCount; }
  }

  public int AnimationIndex
  {
    set
    {
      index = value;
    }
    get { return index; }
  }
	private int index=0;

  void Start(){
    new_texture = renderer.material.mainTexture;
    SetSpriteAnimation();
  }

  public string AnimalTexture
  {
    set
    {
      if (renderer.material.mainTexture.name != value)
      {
        var _texture = Resources.Load(value) as Texture;
        new_texture = _texture;
      }
    }
    get { return renderer.material.mainTexture.name; }
  }
  private Texture new_texture;

  private Vector3 velocity = Vector3.zero;
  public Vector3 Velocity
  {
    get { return velocity; }
    set
    {
      velocity = new Vector3(value.x, value.y, value.z);
      velocity = Vector3.Normalize(velocity) * 2.5f; //* 0.1f;
    }
  }

  void Update()
  {
    if (renderer.material.mainTexture.name != new_texture.name)
    {
      renderer.material.mainTexture = new_texture;
    }

    SetSpriteAnimation();
  }
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }


  public static UnityAnimal Find(string animal_string, int totalAnimations, Vector3 pos)
	{

    var animal = GameObject.Find("/" + animal_string).GetComponent<UnityAnimal>();
    animal.Position = pos;
    animal.AnimationIndex = 0;
    animal.AnimationNumber = totalAnimations;
    return animal;
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               