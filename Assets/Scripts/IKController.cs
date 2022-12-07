using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    LineRenderer lineRenderer0;
    LineRenderer lineRenderer1;
    LineRenderer lineRenderer2;
    GameObject Goal;
    GameObject P0Joint;
    GameObject P1Joint;
    GameObject P2Joint;
    GameObject EndEffector;
    Vector3 P0;
    Vector3 P1;
    Vector3 P2;
    Vector3 E;
    Vector3 G;
    Vector3 dE;
    Vector3 JT0;
    Vector3 JT1;
    Vector3 JT2;
    Vector3 RotAxis;
    float dTheta0;
    float angleTheta0;
    float dTheta1;
    float angleTheta1;
    float dTheta2;
    float angleTheta2;
    Matrix4x4 transMat;
    Matrix4x4 rotMat0;
    Matrix4x4 rotMat1;
    Matrix4x4 rotMat2;
    Vector3 newPosOfP1;
    Vector3 newPosOfP2;
    Vector3 newPosOfE;
    float jointMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer0 = GameObject.Find("Line").GetComponent<LineRenderer>();
        lineRenderer1 = GameObject.Find("Line2").GetComponent<LineRenderer>();
        lineRenderer2 = GameObject.Find("Line3").GetComponent<LineRenderer>();
        P0Joint = GameObject.Find("P0Joint");  
        P1Joint = GameObject.Find("P1Joint");     
        P2Joint = GameObject.Find("P2Joint");    
        EndEffector = GameObject.Find("EndEffector");
        Goal = GameObject.Find("Goal");
        E = EndEffector.transform.position;
        G = Goal.transform.position;
        dE = G - E;
        P0 = P0Joint.transform.position; 
        P1 = P1Joint.transform.position; 
        P2 = P2Joint.transform.position; 
        RotAxis = new Vector3(0,0,1);
        angleTheta0 = 0.0f;
        angleTheta1 = 0.0f;
        angleTheta2 = 0.0f;
        transMat = Matrix4x4.Translate(new Vector3(4,0,0));
        jointMovementSpeed = .15f;

    }

    // Update is called once per frame
    void Update()
    {
        E = EndEffector.transform.position;  
        G = Goal.transform.position;
	    dE = G - E;
        if(dE.magnitude > .01f)
        {

        P0 = P0Joint.transform.position;
        P1 = P1Joint.transform.position;
        P2 = P2Joint.transform.position;
        E = EndEffector.transform.position;  
        G = Goal.transform.position;
	    dE = G - E;
        Vector3[] positions0 = new Vector3[2]{P0, P1};
        Vector3[] positions1 = new Vector3[2]{P1, P2};
        Vector3[] positions2 = new Vector3[2]{P2, E};
        lineRenderer0.positionCount = 2;
        lineRenderer0.SetPositions(positions0); 
        lineRenderer1.positionCount = 2;
        lineRenderer1.SetPositions(positions1); 
        lineRenderer2.positionCount = 2;
        lineRenderer2.SetPositions(positions2); 


    // Jacobian Matrix
        // Theta 0
        JT0 = Vector3.Cross(RotAxis, (E-P0));
        dTheta0 = Vector3.Dot(JT0, dE);
        
        angleTheta0 = angleTheta0 + jointMovementSpeed * dTheta0;
        rotMat0 = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, angleTheta0)));


        // Theta 1
        JT1 = Vector3.Cross(RotAxis, (E-P1));
        dTheta1 = Vector3.Dot(JT1, dE);
        
        angleTheta1 = angleTheta1 + jointMovementSpeed * dTheta1;
        rotMat1 = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, angleTheta1)));

 
        // Theta 2
        JT2 = Vector3.Cross(RotAxis, (E-P2));
        dTheta2 = Vector3.Dot(JT2, dE);
        
        angleTheta2 = angleTheta2 + jointMovementSpeed * dTheta2;
        rotMat2 = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, angleTheta2)));


    // New Positions
    
        newPosOfP1 = (rotMat0 * transMat).MultiplyPoint3x4(Vector3.zero);
        P1Joint.transform.position = newPosOfP1;

        newPosOfP2 = (((rotMat0 * transMat)*rotMat1)*transMat).MultiplyPoint3x4(Vector3.zero);
        P2Joint.transform.position = newPosOfP2;

        newPosOfE = (((((rotMat0 * transMat)*rotMat1)*transMat)*rotMat2)*transMat).MultiplyPoint3x4(Vector3.zero);
        EndEffector.transform.position = newPosOfE;


        }
    }
}
