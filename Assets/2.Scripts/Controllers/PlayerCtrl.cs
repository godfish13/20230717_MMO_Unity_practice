using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    //GameObject (Player) ����

    PlayerStat _Stat;
    Vector3 MouseClickDestination;

    public enum PlayerStatue
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    [SerializeField] PlayerStatue _Statue = PlayerStatue.Idle;

    private void Start()
    {
        /*Managers.inputMgr.KeyAction -= Move; // �ٸ��κп��� Move�� ������������ �׼��̺�Ʈ�� ������ �߻��ϴ� ���׹����� ���� �ʱ�ȭ
        Managers.inputMgr.KeyAction += Move; // inputMgr�� KeyAction�� Move ����*/ // ���ۿ����� �ϴ� ���콺 ������ ����

        _Stat = GetComponent<PlayerStat>();

        Managers.inputMgr.MouseAction -= OnMouseClicked;
        Managers.inputMgr.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        switch (_Statue)
        {
            case PlayerStatue.Die:
                UpdateDie();
                break;
            case PlayerStatue.Idle:
                UpdateIdle();
                break;
            case PlayerStatue.Moving:
                UpdateMoving();
                break;
        }    
    }

    void UpdateDie()
    {
        // �ƹ��͵� ���ϴ� �������
    }

    void UpdateMoving()
    {
        Vector3 dir = MouseClickDestination - transform.position;
        if (dir.magnitude < 0.1f)
        {
            _Statue = PlayerStatue.Idle;
        }
        else
        {
            float MoveDist = Mathf.Clamp(_Stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            // Clamp(float value, min, max) : value���� min�� max ���� �̿��� ���� ���� �ʵ��� �� -> ��Ȯ�� �̵��� �Ÿ� ���
            NavMeshAgent Nma =  gameObject.GetComponent<NavMeshAgent>();
            Nma.Move(dir.normalized * MoveDist);
            // NavMeshAgent.Move(float offset) : offset �������� �̵��ϵ� NavMesh �������� ������

            // ĳ���� ������ ����ĳ�����Ͽ� ��ֹ��� ����� ��� �����ϵ��� ����
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _Statue = PlayerStatue.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // ���� �ִϸ��̼� ó��
        Animator Anim = GetComponent<Animator>();
        // ���� ���� ���¿� ���� ������ �Ѱ���
        Anim.SetFloat("Speed", _Stat.MoveSpeed);
    }

    /*void OnRunEvent()     // AnimationEvent ����
    {
        Debug.Log("�ѹ��ѹ�")
    }*/

    void UpdateIdle()
    {

        // ���� �ִϸ��̼� ó��
        Animator Anim = GetComponent<Animator>();
        Anim.SetFloat("Speed", 0);
    }

    int GroundMonsterLayerMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);
                                // 0000 0000 0010 0000              // 0000 0000 0100 0000
                                // ���� Define���� 6, 7�� ���� ���� �ش� ���鸸ŭ ��Ʈ������ �̵�
                                // �� ���� ���ϸ� 0000 0000 0110 0000 �̹Ƿ� �̴� ���̾� ����ũ���� 6, 7�� ���̾ �ǹ��ϰԵ�
                                // LayerMask.GetMask("String Name")�� �ᵵ �ǳ� ����ӵ��鿡�� ��Ʈ �÷��װ� �ξ� ����
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_Statue == PlayerStatue.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, GroundMonsterLayerMask))
        {
            MouseClickDestination = hit.point;
            _Statue = PlayerStatue.Moving;

            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Monster!!");
            }
            else
            {
                Debug.Log("Ground!!");
            }
        }
    }
}
