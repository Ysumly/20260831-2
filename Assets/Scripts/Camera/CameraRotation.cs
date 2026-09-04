using UnityEngine;
using UnityEngine.Events;

public class CameraRotation : MonoBehaviour
{

    private Animator animator;

    private UnityAction rotationOverCallback;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void LeftRotation(UnityAction callback)
    {
        rotationOverCallback = callback;
        animator.SetTrigger("LeftRotation");
    }

    public void RightRotation(UnityAction callback)
    {
        rotationOverCallback = callback;
        animator.SetTrigger("RightRotation");
    }

    public void RotationOver()
    {
        //动画播放完毕后，执行的逻辑
        //这里可以添加你想要在旋转结束后执行的代码
        rotationOverCallback?.Invoke();
        rotationOverCallback = null; // 清除回调，避免重复调用
    }
}
