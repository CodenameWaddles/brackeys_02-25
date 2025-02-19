using Godot;
using System;
using System.Collections;
using Godot.Collections;

public abstract partial class DataDisplay : Node {
    
    [Export] public int _minimumValue { get; protected set; } = 0;
    [Export] public int _maximumValue { get; protected set; } = 100;
    
    protected Dictionary<int, float> _amplitudeTable = new(){{0, 0.05f}, {1, 0.1f}, {2, 0.15f}, {3, 0.2f}, {4, 0.25f}, {5, 0.3f}};
    protected Dictionary<int, float> _periodTable = new(){{0, 10f}, {1, 12f}, {2, 14f}, {3, 16f}, {4, 18f}, {5, 20f}};
    
    public Array<int> Data { get; protected set; }

    public void IncrementData(int i) {
        if(Data[i] < _maximumValue) {
            Data[i]++;
        }
    }

    public void DecrementData(int i) {
        if(Data[i] > _minimumValue) {
            Data[i]--;
        }
    }

    public void SetData(int i, int value) {
        if (Data[i] < _minimumValue) {
            Data[i] = _minimumValue;
        }
        else if (Data[i] > _maximumValue) {
            Data[i] = _maximumValue;
        }
        else {
            Data[i] = value;
        }
    }
    
    public void ActivateRedLight() {
        // when light is implemented
    }
    
    public void ActivateGreenLight() {
        // when light is implemented
    }
    
    public bool isEqual(DataDisplay dataDisplay)
    {
        for(int i = 0; i < this.Data.Count - 1; i++)
        {
            if (this.Data[i] != dataDisplay.Data[i])
            {
                return false;
            }
        }
        return true;
    }
}