using System;
using System.Linq;
using System.Windows.Forms;
using NLua;
using Rfn.App.Commands;
using Rfn.App.Properties;

namespace Rfn.App.InputBoxes
{
    public class LuaInputBox : IRfnInputBox
    {
        public string ScriptText { get; set; }
        public string ScriptName { get; set; }

        public LuaInputBox() : this("LUADEF")
        {
        }

        public LuaInputBox(string scriptName)
        {
            ScriptName = scriptName;
        }

        public double GetProbability(string value)
        {
            var state = new Lua();
            state["input"] = value;
            double ret;
            try
            {
                var result = state.DoString(ScriptText)[0];
                if (!(result is double dRes))
                    throw new InputBoxException(
                        Resources.LuaInputBox_Exception_WrongReturnType_Text,
                        Resources.LuaInputBox_Exception_LuaError_Caption);
                ret = dRes;
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                throw new InputBoxException(
                    string.Format(Resources.LuaInputBox_Exception_LuaCompileError_Text, e.Message),
                    Resources.LuaInputBox_Exception_LuaError_Caption);
            }

            return ret;
        }

        public string GetKey()
        {
            return $"__{ScriptName}";
        }
    }
}