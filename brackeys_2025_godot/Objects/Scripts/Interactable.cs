using Godot;
using System;

public abstract partial class Interactable : Node {
    
    protected bool _isInteractable;
    public bool IsInteractable { get => _isInteractable; }

    public void Activate() {
        if(_isInteractable) {
            ActivateSpecific();
        }
    }
    
    protected abstract void ActivateSpecific();
    
    public void MakeInteractable() {
        _isInteractable = true;
        MakeInteractableSpecific();
    }
    
    public void MakeUninteractable() {
        _isInteractable = false;
        MakeUninteractableSpecific();
    }
    
    // Override these methods to add specific functionality to MakeInteractable() and MakeUninteractable()
    protected virtual void MakeInteractableSpecific() {}
    protected virtual void MakeUninteractableSpecific() {}

}