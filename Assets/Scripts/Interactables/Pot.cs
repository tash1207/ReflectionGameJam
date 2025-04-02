using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pot : Interactable
{
    Animator animator;

    int animationTest = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    override public void Interact()
    {
        base.Interact();
        performFollowUpAction = false;

        if (Inventory.Instance.HasAllIngredients())
        {
            if (animationTest == 0)
            {
                animator.SetBool("isFull", true);
                animationTest = 1;
                Think("I have all the ingredients for a delicious soup!");
            }
            else if (animationTest == 1)
            {
                animator.SetBool("isSteaming", true);
                animationTest = 2;
                Think("Now we're cooking!");
            }
            else if (animationTest == 2)
            {
                animator.SetBool("isFull", false);
                animator.SetBool("isSteaming", false);
                string[] thoughts = new string[] {
                    "Yummm! I didn't starve!",
                    "I hope my coworkers aren't upset I gave their things away."
                };
                Think(thoughts);
                // Game over behavior
                performFollowUpAction = true;
            }
        }
        else if (Inventory.Instance.HasAllIngredientsExceptLast())
        {
            Think("I have a bunch of ingredients now! If I followed a recipe maybe I could make something yummy.");
        }
        else if (Inventory.Instance.HasASecondaryIngredient())
        {
            string[] thoughts = new string[] {
                "This hypothetical soup is really coming together.",
                "Just a few more ingredients and I can make something good."
            };
            Think(thoughts);
        }
        else if (Inventory.Instance.HasObject(Inventory.InventoryObject.Broth))
        {
            string[] thoughts = new string[] {
                "I have broth so maybe I can make a soup?",
                "I'll definitely need some more ingredients."
            };
            Think(thoughts);
            // TODO: consider changing the interaction to be dropping ingredients into the pot.
        }
        else if (Inventory.Instance.HasObject(Inventory.InventoryObject.Wine))
        {
            Think("The only 'food' I have is a bottle of wine and heated wine does not sound appetizing.");
        }
        else
        {
            string[] thoughts = new string[] {
                "I could use this, if I had any ingredients.",
            };
            Think(thoughts);
            performFollowUpAction = !MirrorManager.Instance.HasEnabledFirstMirror();
        }
    }

    override public void FollowUpAction()
    {
        if (performFollowUpAction)
        {
            if (Inventory.Instance.HasAllIngredients())
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                MirrorManager.Instance.EnableMirror1();
                Think("Crap, I guess duty calls. No need to make others suffer just because I'm hungry.");
            }
        }
    }
}
