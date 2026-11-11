using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        AudioClip[] wardrobeSounds,clickSounds;
        int wardrobeSoundIndex,clickSoundIndex;
        float nextWardrobeSound;
        sealed class StreetSound
        {
            public AudioSource source;
            public AudioClip[] clips;
            public float wait,interval;
            public int next;
        }
        StreetSound[] streetSounds;
        void BuildAdditionalSounds()
        {
            wardrobeSounds=new[]{Clip("equip_tecido_01"),Clip("equip_tecido_02"),Clip("equip_tecido_03")};
            clickSounds=new[]{Clip("ui_click_01"),Clip("ui_click_02")};
            var banks=new[]{
                new[]{Clip("car_pass_01"),Clip("car_pass_02"),Clip("car_pass_03")},
                new[]{Clip("court_ball_01"),Clip("court_ball_02"),Clip("court_squeak_01")},
                new[]{Clip("workshop_tool_01"),Clip("workshop_tool_02")},
                new[]{Clip("barber_clipper_01"),Clip("barber_clipper_02")}
            };
            var positions=new[]{Vector3.zero,new Vector3(-76,1.2f,42),new Vector3(-31.5f,1.5f,5.1f),new Vector3(-98,1.5f,6.8f)};
            streetSounds=new StreetSound[banks.Length];
            for(int i=0;i<banks.Length;i++){
                var node=new GameObject("Som local — "+new[]{"Trânsito","Quadra","Oficina","Barbearia"}[i]);
                node.transform.SetParent(transform,false);node.transform.position=CityView.Origin+positions[i];
                var source=node.AddComponent<AudioSource>();source.playOnAwake=false;source.loop=false;
                source.spatialBlend=1;source.rolloffMode=AudioRolloffMode.Linear;source.minDistance=2;source.maxDistance=i==0?24:16;source.volume=i==0?.24f:.30f;
                streetSounds[i]=new StreetSound{source=source,clips=banks[i],wait=2+i*1.7f,interval=i==0?12:8+i*2};
            }
        }
        AudioClip NextClickSound()
        {return clickSounds==null?Clip("ui_click_01"):clickSounds[clickSoundIndex++%clickSounds.Length];}
        void PlayWardrobeSound()
        {
            if(paused||mutedAudio||Time.unscaledTime<nextWardrobeSound)return;
            nextWardrobeSound=Time.unscaledTime+.12f;
            PlayClip(wardrobeSounds[wardrobeSoundIndex++%wardrobeSounds.Length],.55f);
        }
        void TickAdditionalSounds(float delta)
        {
            bool active=stage==Stage.City&&city!=null&&!city.IsInterior&&city.gameObject.activeInHierarchy&&!paused&&!mutedAudio&&!loadingActive;
            for(int i=0;i<streetSounds.Length;i++){
                var spot=streetSounds[i];
                bool visible=active;
                if(visible&&i==0){visible=city.TryTrafficAudioPosition(out var position);if(visible)spot.source.transform.position=position;}
                if(!visible){if(spot.source.isPlaying)spot.source.Stop();continue;}
                spot.wait=Mathf.Max(0,spot.wait-delta);
                if(spot.wait>0||spot.source.isPlaying||Vector3.Distance(cam.transform.position,spot.source.transform.position)>spot.source.maxDistance)continue;
                spot.source.clip=spot.clips[spot.next++%spot.clips.Length];
                if(spot.source.clip!=null)spot.source.Play();
                spot.wait=spot.interval;
            }
        }
    }
    public partial class CityView
    {
        public bool TryTrafficAudioPosition(out Vector3 position)
        {
            position=traffic==null?Vector3.zero:traffic.position;
            return !IsInterior&&traffic!=null&&traffic.gameObject.activeInHierarchy;
        }
    }
}
