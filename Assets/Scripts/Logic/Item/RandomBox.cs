using UnityEngine;
using System.Collections;

namespace Logic
{
    public class RandomBox : MonoBehaviour
    {
        public int Serial { get; private set; }
        public bool IsInteractive { get; private set; }
        public string ItemId { get; set; }
        private void Awake()
        {
            RandomBoxContainer.Instance.Add(this);
        }

        public void SetSerial(int serial)
        {
            this.Serial = serial;
            GetComponent<SpriteRenderer>().sortingOrder = serial;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.layer != LayerMask.NameToLayer("Character"))
            {
                return;
            }

            IsInteractive = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject.layer != LayerMask.NameToLayer("Character"))
            {
                return;
            }

            IsInteractive = false;
        }

        public void Use()
        {
            RandomBoxContainer.Instance.Remove(this);
            ItemBase createdItem = ItemFactory.Instance.Create(ItemId);
            createdItem.Use(ActorContainer.Instance.LocalCharacter);

            Destroy(this.gameObject);
        }
    }
}