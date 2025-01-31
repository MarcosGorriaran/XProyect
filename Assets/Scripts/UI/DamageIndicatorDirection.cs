using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorDirection : DamageIndicator
{
    [SerializeField]
    Transform _charOrigin;
    [SerializeField]
    DamageIndicatorDirectionNode _baseNode;
    Stack<DamageIndicatorDirectionNode> _nodePool = new Stack<DamageIndicatorDirectionNode>();

    protected override void Awake()
    {
        base.Awake();
        _charHP.onHPChangeBy += OnHarm;
    }

    private void OnHarm(float damage,GameObject source)
    {
        OnHarm(damage);
        if(_nodePool.Count <= 0)
        {
            DamageIndicatorDirectionNode instantiatedNode = Instantiate(_baseNode,transform);
            instantiatedNode.damageSourcePosition = source.transform.position;
            instantiatedNode.onLifeTimeExpire += _nodePool.Push;
            instantiatedNode.victimTransform = _charOrigin;
            if(instantiatedNode.lifeTime == 0)
            {
                instantiatedNode.lifeTime = feedbackDuration;
            }
            
        }
        else
        {
            _nodePool.Pop().gameObject.SetActive(true);
        if(_nodePool.Count <= 0)
        {
            DamageIndicatorDirectionNode instantiatedNode = Instantiate(_baseNode,transform);
            instantiatedNode.damageSourcePosition = source.transform.position;
            instantiatedNode.onLifeTimeExpire += _nodePool.Push;
            instantiatedNode.victimTransform = _charOrigin;
            if(instantiatedNode.lifeTime == 0)
            {
                instantiatedNode.lifeTime = feedbackDuration;
            }
            
        }
        else
        {
            _nodePool.Pop().gameObject.SetActive(true);
        }
        
    }
}
