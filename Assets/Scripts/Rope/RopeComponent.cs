using UnityEngine;
using Rope;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace Rope
{
    public class RopeComponent : MonoBehaviour
    {
        public int Length;
        public Bone Start;
        public Bone End;
        private Bone _junction;
        public GameObject LinkPrefab;
        public GameObject JunctionPrefab;

        public BreakEvent onBreak = new RopeComponent.BreakEvent();

        public bool broken;

        private float _totalFriction = 0;

        private System.DateTime _lastFrictionAdd;
        private System.DateTime? _frictionLimitCrossed;
        private bool _frictionStarted = false;

        private List<Bone> _bones = new List<Bone>();
        public Color baseColor;
        public Color warningColor = Color.red;

        public void Awake()
        {
            _bones.Add(Start);
            _bones.Add(End);
            Bone previousStart = Start;
            Bone previousEnd = End;
            for (int i = 0; i < Length - 1; i+= 2)
            {
                previousStart = NewLink(previousStart, i/2);
                previousEnd = NewLink(previousEnd, Length - i/2);
            }

            GameObject junction = GameObject.Instantiate(JunctionPrefab);
            Bone junctionLink = junction.GetComponent<Bone>();
            junction.name = $"Junction link";
            junction.transform.SetParent(transform);
            junction.transform.position = (Start.transform.position + End.transform.position) / 2;
            junctionLink.Joint.connectedBody = previousStart.rigidbody;
            junctionLink.SecondJoint.connectedBody = previousEnd.rigidbody;

            _junction = junctionLink;
            _bones.Add(_junction);

            broken = false;
            onBreak.AddListener(BreakRope);
        }

        public Bone NewLink(Bone previous, int id)
        {
            GameObject bone = GameObject.Instantiate(LinkPrefab);
            Bone currentBone = bone.GetComponent<Bone>();
            bone.name = $"Link #{id}";
            bone.transform.SetParent(transform);
            bone.transform.position = previous.transform.position + (Vector3)currentBone.Joint.connectedAnchor;
            currentBone.Joint.connectedBody = previous.rigidbody;

            _bones.Add(currentBone);
            return currentBone;
        }

        public void BindStart(Rigidbody2D body)
        {
            if (Start.Joint == null)
            {
                Start.gameObject.AddComponent<HingeJoint2D>();
                HingeJoint2D joint = End.Joint;
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = Vector2.zero;
                joint.anchor = Vector2.zero;
                joint.enableCollision = false;
                joint.connectedBody = body;
            }
            else
            {
                Debug.Log("Start already bound");
            }

        }

        public void BindEnd(Rigidbody2D body)
        {
            if(End.Joint == null)
            {
                End.gameObject.AddComponent<HingeJoint2D>();
                HingeJoint2D joint = End.Joint;
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = Vector2.zero;
                joint.anchor = Vector2.zero;
                joint.enableCollision = false;
                joint.connectedBody = body;
            }
            else
            {
                Debug.Log("End already bound");
            }
        }

        public void UnbindStart()
        {
            Destroy(Start.Joint);
            if(End.Joint == null)
            {
                StartCoroutine(DestroyRope());
            }
        }

        public void UnbindEnd()
        {
            Destroy(End.Joint);

        }

        private void ColorRope(Color color)
        {
            foreach(Bone b in _bones)
            {
                b.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        public void FixedUpdate()
        {
            if (_totalFriction > 20)
            {
                if(_frictionLimitCrossed == null)
                {
                    _frictionLimitCrossed = System.DateTime.Now;
                }
                if (_frictionStarted)
                {
                    System.TimeSpan? delay = (System.DateTime.Now - _frictionLimitCrossed);
                    if (_frictionStarted)
                    {
                        if (delay?.Seconds >= 1)
                        {
                            onBreak.Invoke();
                            _frictionStarted = false;
                        }
                        else
                        {
                            if(delay != null) ColorRope(Color.Lerp(baseColor, warningColor, Mathf.Sqrt((float)delay?.Milliseconds / 1000f)));
                        }
                    }
                }
            }
            if ((System.DateTime.Now - _lastFrictionAdd).Milliseconds > 100)
            {
                _totalFriction = _totalFriction < 0.0001f ? 0f : _totalFriction * 0.5f;
                if(_frictionLimitCrossed != null)
                {
                    if(_totalFriction < 10)
                    {
                        _frictionLimitCrossed = null;
                        _frictionStarted = true;
                        ColorRope(Color.Lerp(baseColor, warningColor, _totalFriction / 50f));
                    }
                }
            }
        }

        #region Rope breaking
        public void AddFriction(float friction)
        {
            _lastFrictionAdd = System.DateTime.Now;
            _totalFriction += friction;
        }
        public class BreakEvent : UnityEvent
        {

        }

        public void BreakRope()
        {
            Debug.Log("Break command registered");
            if (!broken)
            {
                broken = true;
                Destroy(_junction.Joint);
                Debug.Log("Breaking rope");
                StartCoroutine(DropRope());
            }
        }

        IEnumerator<WaitForSeconds> DropRope()
        {
            Debug.Log("Start wait");
            yield return new WaitForSeconds(1);
            Debug.Log("Dropping rope");
            UnbindStart();
            UnbindEnd();
        }

        IEnumerator<WaitForSeconds> DestroyRope()
        {
            Debug.Log("Initiating destroy");
            yield return new WaitForSeconds(10);
            Destroy(this);
        }
        #endregion

    }
}