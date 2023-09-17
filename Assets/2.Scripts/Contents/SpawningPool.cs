using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int MonsterCount;          // ���� ���� ��
    [SerializeField] int ReserveCount;          // ���͸� ������ �����ص� �ڷ�ƾ�� ����(�� ������� ������ ��)
    [SerializeField] int KeepMonsterCount;      // �ʿ� ������ų ������ �� ��

    [SerializeField] Vector3 SpawnPosition;     // ���Ͱ� ������ �߽���
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
            Vector3 RandDir = Random.insideUnitSphere * Random.Range(0, SpawnRadius);    // Random.insideUnitSphere : ������ Vector3 ����
            RandDir.y = 0;
            RandPos = SpawnPosition + RandDir;

            // �� �� �ִ� ��ġ���� �Ǻ�
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(RandPos, path))
                break;
        }

        go.transform.position = RandPos;
        ReserveCount--;
    }
}
