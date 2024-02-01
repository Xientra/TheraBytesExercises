using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SkillTreeNode : MonoBehaviour
{
    public string skillName = "Default Name";
    public bool isLocked = true;
    public bool CanBeUnlocked {
        get
        {
            foreach (SkillTreeNode node in dependencies)
                if (node.isLocked == true)
                    return false;
            return true;
        }
    }

    [SerializeField] private SkillTreeNode[] dependencies;

    // ---------- ========== Visualization ========== ----------
    
    [Header("Visualization:")]
    
    [SerializeField] private TMP_Text label;

    [SerializeField] private GameObject unlockVFX;

    private Image _image;

    [SerializeField] private Color unlockedColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color canBeUnlockedColor = new Color(0.8f, 0.8f, 1, 1);
    [SerializeField] private Color lockedColor = new Color(1, 1, 1, 0.5f);


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnValidate()
    {
        label.text = skillName;
        gameObject.name = "SkillTreeNode (" + skillName + ")";
    }

    private void Update()
    {
        _image.color = isLocked == false ? unlockedColor : (CanBeUnlocked ? canBeUnlockedColor : lockedColor);
    }
    
    public void Unlock()
    {
        if (isLocked == false || CanBeUnlocked == false)
            return;
        
        isLocked = false;
        Instantiate(unlockVFX, transform.position, transform.rotation, transform);
        
    }
}
