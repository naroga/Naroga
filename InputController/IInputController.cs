using UnityEngine;
using System.Collections;

namespace Naroga.InputController
{

    public delegate void SChangeHorizontalAxis(float value);
    public delegate void SChangeVerticalAxis(float value);

    public interface IInputController
    {
        void addHorizontalAxisChangeListener(SChangeHorizontalAxis callback);
        void addVerticalAxisChangeListener(SChangeVerticalAxis callback);
    }

}