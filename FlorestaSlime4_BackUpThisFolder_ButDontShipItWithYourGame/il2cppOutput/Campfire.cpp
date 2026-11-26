#include "pch-cpp.hpp"






struct CancellationTokenSource_tAAE1E0033BCFC233801F8CB4CED5C852B350CB7B;
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3;
struct Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3;
struct LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519;
struct MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71;
struct Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;

IL2CPP_EXTERN_C RuntimeClass* Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C const RuntimeMethod* Component_GetComponent_TisLight_t1E68479B7782AF2050FAA02A5DC612FD034F18F3_mF4816FA12B6F220CA55D47D669D7E50DC118B9E9_RuntimeMethod_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t1FDBFE7EF892ACE9724EB71A293B2E0F744F8E5C 
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	float ___m_value;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 
{
	float ___x;
	float ___y;
	float ___z;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr;
};
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};
struct Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};
struct Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};
struct Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3  : public Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA
{
};
struct MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71  : public Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA
{
	CancellationTokenSource_tAAE1E0033BCFC233801F8CB4CED5C852B350CB7B* ___m_CancellationTokenSource;
};
struct LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519  : public MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71
{
	Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* ___light;
	float ___intensity;
	float ___nextIntensity;
	float ___difference;
	float ___maxFrequency;
	float ___minFrequency;
	float ___lerpSpeed;
	float ___frequency;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___localPosition;
	float ___maxOffset;
	float ___lastTime;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_StaticFields
{
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___zeroVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___oneVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___upVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___downVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___leftVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___rightVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___forwardVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___backVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___positiveInfinityVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___negativeInfinityVector;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif


IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Il2CppSharedGenericObject* Component_GetComponent_TisIl2CppSharedGenericObject_mB7566A31421DEE164644EC1F96FB5EE487B96CC2_gshared (Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3* __this, const RuntimeMethod* method) ;

inline Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* Component_GetComponent_TisLight_t1E68479B7782AF2050FAA02A5DC612FD034F18F3_mF4816FA12B6F220CA55D47D669D7E50DC118B9E9 (Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3* __this, const RuntimeMethod* method)
{
	return ((  Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* (*) (Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3*, const RuntimeMethod*))Component_GetComponent_TisIl2CppSharedGenericObject_mB7566A31421DEE164644EC1F96FB5EE487B96CC2_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float Time_get_time_m3A271BB1B20041144AC5B7863B71AB1F0150374B (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_Store_m81A639686BDC9D9DBE3604FAC89504BBD2AE179C (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_Restore_mE08C0E376335B5CF5F174B719091F4468CE2B54D (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float Light_get_intensity_m8FA28D515853068A93FA68B2148809BBEE4E710F (Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* Component_get_transform_m2919A1D81931E6932C7F06D4C2F0AB8DDA9A5371 (Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Transform_get_localPosition_mA9C86B990DF0685EA1061A120218993FDCC60A95 (Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Light_set_intensity_mE4820C7F39F490B92ED5EA0C3AADA7C0775BE854 (Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* __this, float ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Transform_set_localPosition_mDE1C997F7D79C0885210B7732B4BA50EE7D73134 (Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* __this, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494 (float ___0_minInclusive, float ___1_maxInclusive, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector3__ctor_m376936E6B999EF1ECBE57D990A386303E2283DE0_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* __this, float ___0_x, float ___1_y, float ___2_z, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_op_Addition_m78C0EC70CB66E8DCAC225743D82B268DAEE92067_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___0_a, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___1_b, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_get_one_mC9B289F1E15C42C597180C9FE6FB492495B51D02_inline (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_op_Multiply_m87BA7C578F96C8E49BB07088DAAC4649F83B0353_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___0_a, float ___1_d, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float Time_get_deltaTime_mC3195000401F0FD167DD2F948FD2BC58330D0865 (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Lerp_m47EF2FFB7647BD0A1FDC26DC03E28B19812139B5_inline (float ___0_a, float ___1_b, float ___2_t, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MonoBehaviour__ctor_m592DB0105CA0BC97AA1C5F4AD27B12D68A3B7C1E (MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Clamp01_mA7E048DBDA832D399A581BE4D6DED9FA44CE0F14_inline (float ___0_value, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 74801
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_OnEnable_mB234E94EAA9397695C2BA6905014F34E25FC5F9E (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Component_GetComponent_TisLight_t1E68479B7782AF2050FAA02A5DC612FD034F18F3_mF4816FA12B6F220CA55D47D669D7E50DC118B9E9_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:27>
		Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* L_0;
		L_0 = Component_GetComponent_TisLight_t1E68479B7782AF2050FAA02A5DC612FD034F18F3_mF4816FA12B6F220CA55D47D669D7E50DC118B9E9(__this, Component_GetComponent_TisLight_t1E68479B7782AF2050FAA02A5DC612FD034F18F3_mF4816FA12B6F220CA55D47D669D7E50DC118B9E9_RuntimeMethod_var);
		__this->___light = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___light), (void*)L_0);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:28>
		float L_1;
		L_1 = Time_get_time_m3A271BB1B20041144AC5B7863B71AB1F0150374B(NULL);
		__this->___lastTime = L_1;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:29>
		LightFlicker_Store_m81A639686BDC9D9DBE3604FAC89504BBD2AE179C(__this, NULL);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:30>
		return;
	}
}
// Method Definition Index: 74802
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_OnDisable_m08D13F30E7DFE3F659989C5923EAA190B5C952A6 (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:34>
		LightFlicker_Restore_mE08C0E376335B5CF5F174B719091F4468CE2B54D(__this, NULL);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:35>
		return;
	}
}
// Method Definition Index: 74803
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_Store_m81A639686BDC9D9DBE3604FAC89504BBD2AE179C (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:39>
		Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* L_0 = __this->___light;
		NullCheck(L_0);
		float L_1;
		L_1 = Light_get_intensity_m8FA28D515853068A93FA68B2148809BBEE4E710F(L_0, NULL);
		__this->___intensity = L_1;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:40>
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_2;
		L_2 = Component_get_transform_m2919A1D81931E6932C7F06D4C2F0AB8DDA9A5371(__this, NULL);
		NullCheck(L_2);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_3;
		L_3 = Transform_get_localPosition_mA9C86B990DF0685EA1061A120218993FDCC60A95(L_2, NULL);
		__this->___localPosition = L_3;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:41>
		return;
	}
}
// Method Definition Index: 74804
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_Restore_mE08C0E376335B5CF5F174B719091F4468CE2B54D (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:45>
		Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* L_0 = __this->___light;
		float L_1 = __this->___intensity;
		NullCheck(L_0);
		Light_set_intensity_mE4820C7F39F490B92ED5EA0C3AADA7C0775BE854(L_0, L_1, NULL);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:46>
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_2;
		L_2 = Component_get_transform_m2919A1D81931E6932C7F06D4C2F0AB8DDA9A5371(__this, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_3 = __this->___localPosition;
		NullCheck(L_2);
		Transform_set_localPosition_mDE1C997F7D79C0885210B7732B4BA50EE7D73134(L_2, L_3, NULL);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:47>
		return;
	}
}
// Method Definition Index: 74805
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker_Update_m43074335EE427E23403B6A91987A778C8B86CF39 (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	float V_0 = 0.0f;
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:51>
		float L_0 = __this->___lastTime;
		float L_1 = __this->___frequency;
		float L_2;
		L_2 = Time_get_time_m3A271BB1B20041144AC5B7863B71AB1F0150374B(NULL);
		if ((!(((float)((float)il2cpp_codegen_add(L_0, L_1))) < ((float)L_2))))
		{
			goto IL_00b1;
		}
	}
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:53>
		float L_3;
		L_3 = Time_get_time_m3A271BB1B20041144AC5B7863B71AB1F0150374B(NULL);
		__this->___lastTime = L_3;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:54>
		float L_4 = __this->___intensity;
		float L_5 = __this->___difference;
		float L_6 = __this->___intensity;
		float L_7 = __this->___difference;
		float L_8;
		L_8 = Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494(((float)il2cpp_codegen_subtract(L_4, L_5)), ((float)il2cpp_codegen_add(L_6, L_7)), NULL);
		__this->___nextIntensity = L_8;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:55>
		float L_9 = __this->___minFrequency;
		float L_10 = __this->___maxFrequency;
		float L_11;
		L_11 = Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494(L_9, L_10, NULL);
		__this->___frequency = L_11;
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:57>
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:58>
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:59>
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:60>
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_12;
		L_12 = Component_get_transform_m2919A1D81931E6932C7F06D4C2F0AB8DDA9A5371(__this, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_13 = __this->___localPosition;
		float L_14 = __this->___maxOffset;
		float L_15 = __this->___maxOffset;
		float L_16;
		L_16 = Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494(((-L_14)), L_15, NULL);
		float L_17 = __this->___maxOffset;
		float L_18 = __this->___maxOffset;
		float L_19;
		L_19 = Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494(((-L_17)), L_18, NULL);
		float L_20 = __this->___maxOffset;
		float L_21 = __this->___maxOffset;
		float L_22;
		L_22 = Random_Range_m5236C99A7D8AE6AC9190592DC66016652A2D2494(((-L_20)), L_21, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_23;
		memset((&L_23), 0, sizeof(L_23));
		Vector3__ctor_m376936E6B999EF1ECBE57D990A386303E2283DE0_inline((&L_23), L_16, L_19, L_22, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_24;
		L_24 = Vector3_op_Addition_m78C0EC70CB66E8DCAC225743D82B268DAEE92067_inline(L_13, L_23, NULL);
		NullCheck(L_12);
		Transform_set_localPosition_mDE1C997F7D79C0885210B7732B4BA50EE7D73134(L_12, L_24, NULL);
		goto IL_00e3;
	}

IL_00b1:
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:64>
		float L_25;
		L_25 = Time_get_time_m3A271BB1B20041144AC5B7863B71AB1F0150374B(NULL);
		float L_26;
		L_26 = sinf(L_25);
		V_0 = ((float)il2cpp_codegen_multiply(L_26, (0.00100000005f)));
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:65>
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_27;
		L_27 = Component_get_transform_m2919A1D81931E6932C7F06D4C2F0AB8DDA9A5371(__this, NULL);
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_28 = L_27;
		NullCheck(L_28);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_29;
		L_29 = Transform_get_localPosition_mA9C86B990DF0685EA1061A120218993FDCC60A95(L_28, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_30;
		L_30 = Vector3_get_one_mC9B289F1E15C42C597180C9FE6FB492495B51D02_inline(NULL);
		float L_31 = V_0;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_32;
		L_32 = Vector3_op_Multiply_m87BA7C578F96C8E49BB07088DAAC4649F83B0353_inline(L_30, L_31, NULL);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_33;
		L_33 = Vector3_op_Addition_m78C0EC70CB66E8DCAC225743D82B268DAEE92067_inline(L_29, L_32, NULL);
		NullCheck(L_28);
		Transform_set_localPosition_mDE1C997F7D79C0885210B7732B4BA50EE7D73134(L_28, L_33, NULL);
	}

IL_00e3:
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:68>
		Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* L_34 = __this->___light;
		Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* L_35 = __this->___light;
		NullCheck(L_35);
		float L_36;
		L_36 = Light_get_intensity_m8FA28D515853068A93FA68B2148809BBEE4E710F(L_35, NULL);
		float L_37 = __this->___nextIntensity;
		float L_38 = __this->___lerpSpeed;
		float L_39;
		L_39 = Time_get_deltaTime_mC3195000401F0FD167DD2F948FD2BC58330D0865(NULL);
		float L_40;
		L_40 = Mathf_Lerp_m47EF2FFB7647BD0A1FDC26DC03E28B19812139B5_inline(L_36, L_37, ((float)il2cpp_codegen_multiply(L_38, L_39)), NULL);
		NullCheck(L_34);
		Light_set_intensity_mE4820C7F39F490B92ED5EA0C3AADA7C0775BE854(L_34, L_40, NULL);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:69>
		return;
	}
}
// Method Definition Index: 74806
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LightFlicker__ctor_mC95125983CE6D12B7375BD5815AC38934579965F (LightFlicker_t40D3A3378F914EB369A58655857CF660455E8519* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:13>
		__this->___difference = (1.0f);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:14>
		__this->___maxFrequency = (1.0f);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:15>
		__this->___minFrequency = (0.100000001f);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:16>
		__this->___lerpSpeed = (100.0f);
		//<source_info:D:/Estudos/Unity/Projetos/ForestVirtualEnvironment-VR-/Assets/CampFireAsset/Campfire/Scripts/LightFlicker.cs:21>
		__this->___maxOffset = (0.0500000007f);
		MonoBehaviour__ctor_m592DB0105CA0BC97AA1C5F4AD27B12D68A3B7C1E(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Method Definition Index: 41726
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector3__ctor_m376936E6B999EF1ECBE57D990A386303E2283DE0_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* __this, float ___0_x, float ___1_y, float ___2_z, const RuntimeMethod* method) 
{
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:331>
		float L_0 = ___0_x;
		__this->___x = L_0;
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:331>
		float L_1 = ___1_y;
		__this->___y = L_1;
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:331>
		float L_2 = ___2_z;
		__this->___z = L_2;
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:331>
		return;
	}
}
// Method Definition Index: 41764
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_op_Addition_m78C0EC70CB66E8DCAC225743D82B268DAEE92067_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___0_a, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___1_b, const RuntimeMethod* method) 
{
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:778>
		il2cpp_codegen_initobj((&V_0), sizeof(Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_0 = ___0_a;
		float L_1 = L_0.___x;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_2 = ___1_b;
		float L_3 = L_2.___x;
		(&V_0)->___x = ((float)il2cpp_codegen_add(L_1, L_3));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_4 = ___0_a;
		float L_5 = L_4.___y;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_6 = ___1_b;
		float L_7 = L_6.___y;
		(&V_0)->___y = ((float)il2cpp_codegen_add(L_5, L_7));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_8 = ___0_a;
		float L_9 = L_8.___z;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_10 = ___1_b;
		float L_11 = L_10.___z;
		(&V_0)->___z = ((float)il2cpp_codegen_add(L_9, L_11));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_12 = V_0;
		return L_12;
	}
}
// Method Definition Index: 41757
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_get_one_mC9B289F1E15C42C597180C9FE6FB492495B51D02_inline (const RuntimeMethod* method) 
{
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:728>
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_0 = ((Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_StaticFields*)il2cpp_codegen_static_fields_for(Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_il2cpp_TypeInfo_var))->___oneVector;
		return L_0;
	}
}
// Method Definition Index: 41767
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Vector3_op_Multiply_m87BA7C578F96C8E49BB07088DAAC4649F83B0353_inline (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___0_a, float ___1_d, const RuntimeMethod* method) 
{
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Vector3.cs:787>
		il2cpp_codegen_initobj((&V_0), sizeof(Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_0 = ___0_a;
		float L_1 = L_0.___x;
		float L_2 = ___1_d;
		(&V_0)->___x = ((float)il2cpp_codegen_multiply(L_1, L_2));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_3 = ___0_a;
		float L_4 = L_3.___y;
		float L_5 = ___1_d;
		(&V_0)->___y = ((float)il2cpp_codegen_multiply(L_4, L_5));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_6 = ___0_a;
		float L_7 = L_6.___z;
		float L_8 = ___1_d;
		(&V_0)->___z = ((float)il2cpp_codegen_multiply(L_7, L_8));
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_9 = V_0;
		return L_9;
	}
}
// Method Definition Index: 41873
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Lerp_m47EF2FFB7647BD0A1FDC26DC03E28B19812139B5_inline (float ___0_a, float ___1_b, float ___2_t, const RuntimeMethod* method) 
{
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Mathf.cs:308>
		float L_0 = ___0_a;
		float L_1 = ___1_b;
		float L_2 = ___0_a;
		float L_3 = ___2_t;
		float L_4;
		L_4 = Mathf_Clamp01_mA7E048DBDA832D399A581BE4D6DED9FA44CE0F14_inline(L_3, NULL);
		return ((float)il2cpp_codegen_add(L_0, ((float)il2cpp_codegen_multiply(((float)il2cpp_codegen_subtract(L_1, L_2)), L_4))));
	}
}
// Method Definition Index: 41872
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Clamp01_mA7E048DBDA832D399A581BE4D6DED9FA44CE0F14_inline (float ___0_value, const RuntimeMethod* method) 
{
	{
		//<source_info:/home/bokken/build/output/unity/unity/Runtime/Export/Math/Mathf.cs:302>
		float L_0 = ___0_value;
		if ((((float)L_0) < ((float)(0.0f))))
		{
			goto IL_0018;
		}
	}
	{
		float L_1 = ___0_value;
		if ((((float)L_1) > ((float)(1.0f))))
		{
			goto IL_0012;
		}
	}
	{
		float L_2 = ___0_value;
		return L_2;
	}

IL_0012:
	{
		return (1.0f);
	}

IL_0018:
	{
		return (0.0f);
	}
}
