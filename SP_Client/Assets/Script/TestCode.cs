/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

	void Start () {
        string path = Application.dataPath + @"\TokyoLive_QuestionBook.csv";
        List<Dictionary<string, object>> data = CSVReader.Read(path);

        for (int i = 0; i < data.Count; i++)
        {
            foreach (KeyValuePair<string, object> pair in data[i])
                Debug.Log(pair.Key + ": " + pair.Value);
        }
	}


}*/

//---------------------------------------------------
//
// Copyright © 2014     berabue@gmail.com
//
//---------------------------------------------------

using UnityEngine;
using System.Collections;

public class QbRollingNumbers : MonoBehaviour {

	public GUIText[]    m_Label;
	public float        m_During = 1.0f;

	private float[]     m_fValue;

	void Awake() {
		int len = m_Label.Length;

		if ( len != 0 ) {
			m_fValue = new float[len];
		}
	}

	/// <summary>
	/// 롤링을 사용하지 않고 값 초기화.
	/// </summary>
	public void InitValue(int _pos, int _value) {
		m_Label[_pos].text = _value.ToString();
	}

	/// <summary>
	/// During(시간)을 사용하여 갱신 요청.
	/// </summary>
	public void MoveToTime(int _pos, int _value) {
		m_fValue[_pos] = _value;

		StartCoroutine("UpdateMoveToTime", _pos);
	}

	private IEnumerator UpdateMoveToTime(int _pos) {
		float value     = m_fValue[_pos];                   // 최종 값
		float prevValue = float.Parse(m_Label[_pos].text);  // 현재 표시 값(변경 이전 값)
		float distance  = Abs(prevValue - value);           // 두 값 사이의 거리
		float limitTime = m_During;                         // 해당 시간 동안 롤링

		while(true) {
			yield return null;

			// deltaTime동안 이동 해야 할 거리를 계산
			float dis = distance * (Time.deltaTime / m_During);

			// 라벨에 쓰여진 숫자(prevValue)에서 목표 값(value)까지 dis 만큼 이동한 값을 얻는다
			prevValue = GetMovedValue(prevValue, value, dis);

			// 얻은 값을 라벨에 입력
			m_Label[_pos].text = ((int)prevValue).ToString();

			// 지정 된 시간이 지나면 롤링 종료
			if ( limitTime < 0.0f )
				break;

			limitTime -= Time.deltaTime;
		}

		// 예외처리
		// 값이 잘못 입력되었을 때를 대비한다
		m_Label[_pos].text = value.ToString();
	}

	// 절대값.
	private float Abs(float _value) {
		return ( _value > 0 ) ? _value : -(_value);
	}

	// _distance 만큼 이동 된 결과를 return.
	private float GetMovedValue(float _start, float _end, float _distance) {
		// 값이 동일하면 목표값을 return
		if ( _start == _end )
			return _end;

		// 시작점과 도착점의 거리 차이를 구함
		float value = Abs(_start - _end);

		// 두 지점 차이의 거리가 이동 해야 할 거리보다 적으면 목표 값 return
		if ( value < _distance )
			return _end;

		// 시작 값이 목표 값보다 작을 경우 시작값에 이동거리를 더하여 return
		if ( _start < _end )
			return _start + _distance;

		// 시작 값이 목표 값보다 클 경우 시작값에 이동거리를 빼고 return
		return _start - _distance;
	}
}
