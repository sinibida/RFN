using Rfn.App.Properties;

namespace Rfn.App.InputBoxes.Lua
{
    public class LuaInputBox : IRfnInputBox
    {
        public string Name { get; set; }
        public string ScriptText { get; set; }
        public string Key { get; set; }
        public int Order { get; set; }

        public LuaInputBox()
        {
        }

        public double GetProbability(string value)
        {
            var state = new NLua.Lua();
            state["input"] = value;
            double ret;
            try
            {
                var result = state.DoString(ScriptText)[0];
                if (!(result is double dRes))
                    throw new InputBoxException(
                        this,
                        Resources.LuaInputBox_Exception_WrongReturnType_Text,
                        GetMsgBoxCaption());
                ret = dRes;
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                throw new InputBoxException(
                    this,
                    string.Format(Resources.LuaInputBox_Exception_LuaCompileError_Text, e.Message),
                    GetMsgBoxCaption());
            }

            return ret;
        }

        private string GetMsgBoxCaption()
        {
            return string.Format(Resources.LuaInputBox_Exception_LuaError_Caption, Name);
        }

        public string GetKey() => Key;
        public int GetOrder() => Order;
    }
}