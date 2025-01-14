using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class PlayerAnimationSetup : MonoBehaviour
{
    public Sprite[] dashSprites;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Create and add animations for 2 directions
        CreateAndAddAnimation("Dash_Left", dashSprites, new int[] { 0, 7, 6, 5, 4, 3, 2 }, 0.03f, true);
        CreateAndAddAnimation("Dash_Right", dashSprites, new int[] { 4, 3, 2, 1, 0, 7, 6 }, 0.03f, true);
    }

    void CreateAndAddAnimation(string name, Sprite[] sprites, int[] frameIndices, float frameTime, bool loop)
    {
        AnimationClip clip = new AnimationClip();
        clip.frameRate = 1.0f / frameTime;

        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frameIndices.Length];
        for (int i = 0; i < frameIndices.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = i * frameTime;
            keyFrames[i].value = sprites[frameIndices[i]];
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyFrames);

        if (loop)
        {
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
        }

        AssetDatabase.CreateAsset(clip, "Assets/Animations/" + name + ".anim");

        // Add the clip to the Animator Controller
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController != null)
        {
            AnimatorState state = animatorController.AddMotion(clip);
            state.name = name;
        }
    }
}