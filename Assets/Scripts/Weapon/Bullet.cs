
using UnityEngine;
using UnityEditor;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        public ImpackAudioData ImpackAudioData;
        private Transform bulletTransform;
        private Vector3 prevPosition;
        //private Rigidbody bulletRigibody;
        private void Start()
        {
            //bulletRigibody = GetComponent<Rigidbody>();
            bulletTransform = transform;
            prevPosition = bulletTransform.position;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;
            bulletTransform.Translate(0, 0, BulletSpeed * Time.deltaTime);
            //受击特效
            if (!Physics.Raycast(prevPosition, (bulletTransform.position - prevPosition).normalized,
                out RaycastHit hit, (bulletTransform.position - prevPosition).magnitude)) return;

            var tmp_BulletEffect = Instantiate(ImpactPrefab, hit.point,
            Quaternion.LookRotation(hit.normal, Vector3.up));
            Object.Destroy(tmp_BulletEffect, 3f);

            //音频
            var tmp_tagswithAudio = ImpackAudioData.ImpackTagsWithAduios.Find((tmp_AudioData) =>
            {
                return tmp_AudioData.Tag.Equals(hit.collider.tag);//获取目标标签
            });
            if (tmp_tagswithAudio == null) return;
            int tmp_Length = tmp_tagswithAudio.ImpackAudioClip.Count;
            AudioClip tmp_audio = tmp_tagswithAudio.ImpackAudioClip[Random.Range(0, tmp_Length)];
            AudioSource.PlayClipAtPoint(tmp_audio, hit.point);


        }
    }
}