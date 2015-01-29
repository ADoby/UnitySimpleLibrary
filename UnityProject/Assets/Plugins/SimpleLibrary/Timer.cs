using Mathf = UnityEngine.Mathf;
using AnimationCurve = UnityEngine.AnimationCurve;
using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

namespace SimpleLibrary
{
    [System.Serializable]
    public class Timer
    {
        #region Editor
        public bool Foldout = false;
        #endregion

        public enum TimerType
        {
            CONST,
            RANDOM_CURVE,
            RANDOM_BETWEEN_TWO_CONSTANTS,
            RANDOM_BETWEEN_TWO_CURVES
        }

        public TimerType MyType;

        //CONST
        public float Time1 = 0f;

        //RANDOM CURVE
        public AnimationCurve Curve1 = AnimationCurve.Linear(0, 0, 1, 1);
        public float ValueMultiplier = 1f;

        //RANDOM TWO CONSTANTS
        public float Time2 = 0f;

        //RANDOM TWO CRUVES
        public AnimationCurve Curve2 = AnimationCurve.Linear(0, 0, 1, 1);

        public float CurrentTimeValue = 0f;
        public float timer = 0f;

        public Timer()
        {
            Reset();
        }

        public virtual float CurrentTime
        {
            get
            {
                return timer;
            }
        }

        public virtual void Start()
        {
            Reset();
        }

        public virtual void Reset()
        {
            switch (MyType)
            {
                case TimerType.CONST:
                    CurrentTimeValue = Time1;
                    break;
                case TimerType.RANDOM_CURVE:
                    CurrentTimeValue = Curve1.Evaluate(RandomCurveTime(ref Curve1)) * ValueMultiplier;
                    break;
                case TimerType.RANDOM_BETWEEN_TWO_CONSTANTS:
                    CurrentTimeValue = Random.Range(Time1, Time2);
                    break;
                case TimerType.RANDOM_BETWEEN_TWO_CURVES:
                    float time = RandomCurveTime(ref Curve1);
                    float curve2Min = CurveMinTime(ref Curve2);
                    float curve2Max = CurveMaxTime(ref Curve2);
                    time = Mathf.Clamp(time, curve2Min, curve2Max);
                    CurrentTimeValue = Random.Range(Curve1.Evaluate(time), Curve2.Evaluate(time)) * ValueMultiplier;
                    break;
                default:
                    CurrentTimeValue = Time1;
                    break;
            }
            timer = 0f;
        }
        protected virtual float RandomCurveTime(ref AnimationCurve curve)
        {
            return Random.Range(CurveMinTime(ref curve), CurveMaxTime(ref curve));
        }
        protected virtual float CurveMinTime(ref AnimationCurve curve)
        {
            return curve.keys[0].time;
        }
        protected virtual float CurveMaxTime(ref AnimationCurve curve)
        {
            return curve.keys[curve.keys.Length - 1].time;
        }

        public virtual void Finish()
        {
            timer = CurrentTimeValue;
        }

        public virtual bool Update()
        {
            return Add(Time.deltaTime);
        }
        public virtual bool UpdateAutoReset()
        {
            return AddAutoReset(Time.deltaTime);
        }

        public virtual bool Add(float amount)
        {
            timer = Mathf.Min(timer + amount, CurrentTimeValue);
            return Finished;
        }
        public virtual bool AddAutoReset(float amount)
        {
            if (Add(amount))
            {
                Reset();
                return true;
            }
            return false;
        }

        public virtual float Procentage
        {
            get
            {
                if (CurrentTimeValue == 0)
                    return 1f;
                return Mathf.Clamp01(timer / CurrentTimeValue);
            }
            protected set
            {

            }
        }
        public virtual bool Finished
        {
            get
            {
                return Procentage == 1f;
            }
        }
    }
}