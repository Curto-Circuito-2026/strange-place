using UnityEngine;

public class DamageScript : MonoBehaviour
{
    

    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // puxa o script de dano :)
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
