/*  ╔═══════════════════════════════════════════════════╡  BallisticsComputer 2018 ╞══════════════════════════════════════════════╗            
    ║ Authors:  Dmitrii Roets                       Email:    roetsd@icloud.com                                                   ║
    ╟─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╢░ 
    ║ Purpose:  Computes the needed Vector3 launch velocity needed to hit a world space coordinate target                         ║░
    ║ Usage:    Vector3 velocityNew = BallisticsComputer.CalculateVelocityAngleOfLunch(target.position, barrelOut.position, 100); ║░
                bullet.GetComponent<Rigidbody>().AddForce(velocityNew,ForceMode.VelocityChange);                                  ║░
    ╚═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝░
       ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
*/
using UnityEngine;

public enum ballistic_Mode : byte { highestShot, mostDirect, lowestSpeedArc };

public class BallisticsComputer  {

    public static Vector3 CalculateVelocityAngleOfLunch(Vector3 _target, Vector3 _position, float _speed, ballistic_Mode _ballisticMode = ballistic_Mode.mostDirect)
    {
        float T = 0;
        Vector3 toTarget = _target - _position;                             // get direction to target 
        float gravitySquared = Physics.gravity.sqrMagnitude;                // cache the value for efficiency

        // solving the quadratic equasion
        float b = _speed * _speed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gravitySquared * toTarget.sqrMagnitude;

        if (discriminant < 0)
        {
            Debug.Log("<Color=Maroon><b>BallisticsComputer</b></Color>: <Color=Yellow>Cant Reach target</Color> , _speed of (" + _speed.ToString("F2") + " insufficient, will fire at max possible speed"); 
            T = Mathf.Sqrt((b) * 2f / gravitySquared);
            return (toTarget / T - Physics.gravity * T / 2f);
        }

        switch (_ballisticMode)
        {
            case ballistic_Mode.highestShot:     T = Mathf.Sqrt((b + Mathf.Sqrt(discriminant)) * 2f   / gravitySquared);      break;
            case ballistic_Mode.mostDirect:      T = Mathf.Sqrt((b - Mathf.Sqrt(discriminant)) * 2f   / gravitySquared);      break;
            case ballistic_Mode.lowestSpeedArc:  T = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gravitySquared));     break;
        }

        return toTarget / T - Physics.gravity * T / 2f;
    }
}
