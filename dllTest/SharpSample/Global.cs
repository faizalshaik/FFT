using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using fftlib;
class Global
{
    static Global m_s = null;
    static public Global instance()
    {
        if(m_s==null)
            m_s = new Global();
        return m_s;
    }
    public OutPutFileReader reader = new OutPutFileReader();
}

