var rotateSpeed = 5.0;
var billboarded : boolean;
var rotating : boolean;

function Update() {
	if (billboarded == true)
    transform.LookAt(Camera.main.transform); 
    
    if (rotating == true)
    transform.Rotate(Vector3(0,0,1), rotateSpeed * Time.deltaTime);
}