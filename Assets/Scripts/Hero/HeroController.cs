using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HeroController : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;
    HeroInventory heroInventory;

    public float speed;
    public float distance;

    public Vector3 target;
    public Transform targetInteract;

    public string act;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heroInventory = GetComponent<HeroInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        { 
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100f))
            {
                ClickUpdate(hit);
            }
        }

        switch (act)
        {
            case "Move": Move(); break;
            case "Attack": Attack(); break;
            case "": Null(); break;
        }
    }

    void ClickUpdate(RaycastHit hit)
    {
        if(hit.transform.tag == "Ground")
        {
            target = hit.point;
            targetInteract = null;
            act = "Move";
        }
        else if(hit.transform.tag == "Item")
        {
            targetInteract = null;
            TakeItem(hit);
        }
        else if (hit.transform.tag == "Enemy")
        {
            if (heroInventory.weaponInHand != null)
            {
                targetInteract = hit.transform;
                act = "Attack";
            }
        }
    }

    void Move()
    {
        
        distance = Vector3.Distance(transform.position, target);
        anim.SetFloat("Speed", speed);
        speed = Mathf.Clamp(speed, 0, 1);

        if (distance > 0.5f)
        {
            agent.SetDestination(target);
            agent.isStopped = false;
            speed += 2 * Time.deltaTime;
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
        }
        else if (distance <= 0.5f)
        {
            speed -= 5 * Time.deltaTime;
            //speed = Mathf.Clamp(speed, 0, 1); Временно убрано №9

            if (speed <= 0.2f)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", false);
                agent.isStopped = true;
                act = "";
            }
        }

        
    }

    void Attack()
    {
        distance = Vector3.Distance(transform.position, targetInteract.position);
        anim.SetFloat("Speed", speed);
        speed = Mathf.Clamp(speed, 0, 1);

        if (distance > 2f)
        {
            agent.SetDestination(targetInteract.position);
            agent.isStopped = false;
            speed += 2 * Time.deltaTime;
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
        }
        else if (distance <= 2f)
        {
            speed -= 5 * Time.deltaTime;
            //speed = Mathf.Clamp(speed, 0, 1); Временно убрано №9

            Vector3 direction = (targetInteract.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

            if (speed <= 0.2f)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", true);
                agent.isStopped = true;
            }
        }
    }

    void Null()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
    }

    void TakeItem(RaycastHit hit)
    {
        distance = Vector3.Distance(transform.position + transform.up, hit.transform.position);

        Item it = hit.transform.GetComponent<Item>();

        if (distance < 2.3)
        {
            if(it.typeItem == "Food")
            {
                heroInventory.food.Add(hit.transform.GetComponent<Item>());
                Destroy(hit.transform.gameObject);
            }
            else if(it.typeItem == "Weapon")
            {
                heroInventory.weapon.Add(hit.transform.GetComponent<Item>());
                Destroy(hit.transform.gameObject);
            }

        }
        else
        {
            print("Далеко " + distance); //Для теста
        }
    }

    public void Damage()
    {
        targetInteract.GetComponent<NPCStats>().TakeAwayHealth(heroInventory.mainWeapon.item.damage);
        
        if(targetInteract.GetComponent<NPCStats>().health <= 0)
        {
            act = "";
        }
    }
}
