using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JSMinify

{

    public class Minify

    {

       private string mFileName = "";              

       private string mOriginalData = "";           

        private string mModifiedData = "";          

        private bool mIsError = false;               
   
        private string mErr = "";                    

        private BinaryReader mReader = null;         

 

        private const int EOF = -1;                 

 

        

        public Minify(string f)
{
   



            try
       {
       
                if (File.Exists(f))
          
                {
            
                    mFileName = f;
            




                    StreamReader rdr = new StreamReader(mFileName);
            
                    mOriginalData = rdr.ReadToEnd();
            
                    rdr.Close();
            



                    mReader = new BinaryReader(new FileStream(mFileName, FileMode.Open));
            
                    doProcess();
            
                    mReader.Close();
            


                    

                    string outFile = mFileName + ".min";
            
                    StreamWriter wrt = new StreamWriter(outFile);
            
                    wrt.Write(mModifiedData);
            
                    wrt.Close();
            



                }
        
                else
        {
            
                    mIsError = true;
            
                    mErr = "File does not exist";
            
                }
        



            }
    
            catch (Exception ex)
    {
        
                mIsError = true;
        
                mErr = ex.Message;
        
            }
    
        }


        private void doProcess()
{
    
            int lastChar = 1;                   
    
            int thisChar = -1;                  
    
            int nextChar = -1;                  
    
          bool endProcess = false;            
    
            bool ignore = false;                
    
            bool inComment = false;             
    
            bool isDoubleSlashComment = false;  
    






            

            while (!endProcess)
    {
        
                endProcess = (mReader.PeekChar() == -1);   
        
                if (endProcess)
            
                    break;
        

                ignore = false;
        
                thisChar = mReader.ReadByte();
        



                if (thisChar == '\t')
            
                    thisChar = ' ';
        
                else if (thisChar == '\t')
            
                    thisChar = '\n';
        
                else if (thisChar == '\r')
            
                    thisChar = '\n';
        



                if (thisChar == '\n')
            
                    ignore = true;
        



                if (thisChar == ' ')
            
                {
            
                    if ((lastChar == ' ') || isDelimiter(lastChar) == 1)
                
                        ignore = true;
            
                    else
            {
                
                        endProcess = (mReader.PeekChar() == -1); 
                
                        if (!endProcess)
                    
                        {
                    
                            nextChar = mReader.PeekChar();
                    
                            if (isDelimiter(nextChar) == 1)
                        
                                ignore = true;
                    
                        }
                
                    }
            
                }
        






                if (thisChar == '/')
            
                {
            
                    nextChar = mReader.PeekChar();
            
                    if (nextChar == '/' || nextChar == '*')
                
                    {
                
                        ignore = true;
                
                        inComment = true;
                
                        if (nextChar == '/')
                    
                            isDoubleSlashComment = true;
                
                        else
                    
                            isDoubleSlashComment = false;
                
                    }
            






                }
        



                
                if (inComment)
        {
            
                    while (true)
            {
                
                        thisChar = mReader.ReadByte();
                
                        if (thisChar == '*')
                {
                    
                            nextChar = mReader.PeekChar();
                    
                            if (nextChar == '/')
                        
                           {
                        
                                thisChar = mReader.ReadByte();
                        
                                inComment = false;
                        
                                break;
                        
                            }
                    
                        }
                
                        if (isDoubleSlashComment && thisChar == '\n')
                {
                    
                                inComment = false;
                    
                                break;
                    
                        }
                



                     } 
                    ignore = true;
            
                } 
        






                if (!ignore)
            
                    addToOutput(thisChar);
        



                lastChar = thisChar;
        
            } 
    
        }

        private void addToOutput(int c)

        {

            mModifiedData += (char) c;

        }




        public string getOriginalData()

        {
    
            return mOriginalData;
    
        }

        public string getModifiedData()

        {
    
            return mModifiedData;
    
        }



      
        private int isAlphanumeric(int c)

        {
    
            int retval = 0;
    



            if ((c >= 'a' && c <= 'z') ||

                (c >= '0' && c <= '9') ||

                (c >= 'A' && c <= 'Z') ||

                c == '_' || c == '$' || c == '\\' || c > 126)

                retval = 1;
    



            return retval;
    



        }




        private int isDelimiter(int c)

        {
    
            int retval = 0;
    



            if (c == '(' || c == ',' || c == '=' || c == ':' ||

                c == '[' || c == '!' || c == '&' || c == '|' ||

                c == '?' || c == '+' || c == '-' || c == '~' ||

                c == '*' || c == '/' || c == '{' || c == '\n' ||

                c == ',')

            {
        
                retval = 1;
        
            }
 
            return retval;
     }

 }

}
