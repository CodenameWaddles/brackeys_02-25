using Godot;
using System;

public partial class InputButton : Interactable {

    public enum ButtonType {
        Increment,
        Decrement
    }

    [Export] ButtonType _type;
    [Export] DataDisplay _data;
    [Export] CollisionShape3D _shape;
    [Export] int _dataIndex;
    
    public override void _Ready() {
        MakeInteractable();
    }
    
    protected override void ActivateSpecific() {
        if(_type == ButtonType.Increment) {
            _data.IncrementData(_dataIndex);
        } else {
            _data.DecrementData(_dataIndex);
        }
    }

    protected override void MakeInteractableSpecific() {
        // Make button interactable
        _shape.Disabled = false;
    }
    
    protected override void MakeUninteractableSpecific() {
        // Make button uninteractable
        _shape.Disabled = true;
    }
}