//@AddComponentMenu("planet/CloudsRotate")
//partial class CloudsRotate{ }

private var amtx : float =  0.0;
private var amty : float =  0.0;
private var amtz : float =  0.0;
//private var amtx : float = Random.Range(.2,-.2);
//private var amtz : float = Random.Range(.2,-.2);
    
private var this_transform : Transform;

function Start()
{
	amty  = Random.Range(Random.Range(4.0,7.0),-Random.Range(4.0,7.0));
	amtx  = Random.Range(Random.Range(4.0,7.0),-Random.Range(4.0,8.0));
	amty  = Random.Range(Random.Range(4.0,7.0),-Random.Range(4.0,8.0));
	this_transform = transform;	
}

function Update () 
{

	this_transform.Rotate(Time.deltaTime *amtx, Time.deltaTime *amty, Time.deltaTime *amtz);
}