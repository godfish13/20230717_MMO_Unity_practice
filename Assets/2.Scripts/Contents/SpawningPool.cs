using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int MonsterCount;          // 현재 몬스터 수
    [SerializeField] int ReserveCount;          // 몬스터를 만들라고 예약해둔 코루틴의 갯수(곧 만들어질 몬스터의 수)
    [SerializeField] int KeepMonsterCount;      // 맵에 유지시킬 몬스터의 총 수

    [SerializeField] Vector3 SpawnPosition;     // 몬스터가 나오는 중심점
    [SerializeField] float SpawnRadius = 15.0f;
    [SerializeField] float SpawnTime = 5.0f;

    public void AddMonsterCount(int value) => MonsterCount += value;
    public void SetKeepMonsterCount(int value) => KeepMonsterCount = value;

    void Start()
    {
        Managers.gameMgr.OnSpawnEvent -= AddMonsterCount;
        Managers.gameMgr.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (ReserveCount + MonsterCount < KeepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    } 

    IEnumerator ReserveSpawn()
    {
        ReserveCount++;
        yield return new WaitForSeconds(Random.Range(0, SpawnTime));
        GameObject go = Managers.gameMgr.Spawn(Define.WorldObject.Monster, "DogPBR");
        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>();
        Vector3 RandPos;

        while (true)
        {
            Vector3 RandDir = Random.insideUnitSphere * Random.Range(0, SpawnRadius);    // Random.insideUnitSphere : 랜덤한 Vector3 생성
            RandDir.y = 0;
            RandPos = SpawnPosition + RandDir;

            // 갈 수 있는 위치인지 판별
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(RandPos, path))
                break;
        }

        go.transform.position = RandPos;
        ReserveCount--;
    }
}
