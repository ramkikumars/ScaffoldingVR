using UnityEngine;
using SG;

public class Remove : MonoBehaviour
{
    public GameObject tapeComponent;

    public bool removeComponent;
    public bool addComponent;
    public bool setAxis;

    private void Update()
    {
        if (removeComponent)
        {
            RemoveSimpleDrawerComponent();
            removeComponent = false; // Reset the value after invoking the function
        }

        if (addComponent)
        {
            AddGrabableComponent();
            addComponent = false; // Reset the value after invoking the function
        }

        if (setAxis)
        {
            SetSimpleDrawerAxisX();
            setAxis = false; // Reset the value after invoking the function
        }
    }

    public void RemoveSimpleDrawerComponent()
    {
        SG_Grabable componentToRemove = tapeComponent.GetComponent<SG_Grabable>();

        if (componentToRemove != null)
        {
            Destroy(componentToRemove);
        }
    }

    public void RemoveGrabableComponent()
    {
        SG_SimpleDrawer componentToRemove = tapeComponent.GetComponent<SG_SimpleDrawer>();

        if (componentToRemove != null)
        {
            Destroy(componentToRemove);
        }
    }

    public void AddGrabableComponent()
    {
        if (tapeComponent.GetComponent<SG_Grabable>() == null)
        {
            tapeComponent.AddComponent<SG_Grabable>();
        }
    }

    public void AddSimpleDrawerComponent()
    {
        if (tapeComponent.GetComponent<SG_SimpleDrawer>() == null)
        {
            tapeComponent.AddComponent<SG_SimpleDrawer>();
        }
    }

    public void SetSimpleDrawerAxisX()
    {
        SG_SimpleDrawer simpleDrawerComponent = tapeComponent.GetComponent<SG_SimpleDrawer>();

        if (simpleDrawerComponent != null)
        {
            simpleDrawerComponent.moveAxis = SG_SimpleDrawer.DrawerAxis.X;
        }
    }
}
