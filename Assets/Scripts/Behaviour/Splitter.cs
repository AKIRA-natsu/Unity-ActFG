using UnityEngine;
using EzySlice;

public class Splitter : MonoBehaviour {
    public LayerMask mask;
    private Vector3 size;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        size = this.transform.GetSize();
    }
    
    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        transform.Rotate(0, 0, -mx);

        if (Input.GetMouseButtonDown(0)) {
            Collider[] colliders = Physics.OverlapBox(this.transform.position, size, this.transform.rotation, mask);

            foreach (var c in colliders) {
                c.gameObject.Destory();
                var hull = c.gameObject.Slice(this.transform.position, this.transform.up);
                if (hull == null)
                    continue;
                GameObject[] objs = new GameObject[] {
                    hull.CreateLowerHull(c.gameObject, material),
                    hull.CreateUpperHull(c.gameObject, material),
                };

                foreach (var obj in objs) {
                    obj.AddComponent<BoxCollider>();
                    var rigid = obj.AddComponent<Rigidbody>();
                    obj.layer = c.gameObject.layer;
                    rigid.AddExplosionForce(100, c.transform.position, 10);
                }
            }
        }
    }
}
