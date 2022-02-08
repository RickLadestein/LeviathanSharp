using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using OpenTK.Audio.OpenAL;
namespace Leviathan.Core.Windowing
{
    public unsafe struct NativeWindow
    {
        public String w_title;
        public Vector2d w_position;
        public Vector2d w_size;
        public bool w_hasfocus;
        public bool w_isIconified;

        public AudioContext audio_context;

        public unsafe WindowHandle* w_handle;
        public unsafe GlfwNativeWindow n_window;
        public IntPtr _hwinst;

        public Glfw _glfwcontext;
        public GL _glcontext;


        public static unsafe NativeWindow CreateWindow(uint width, uint height, string title, WindowMode mode)
        {
            NativeWindow output = new NativeWindow();
            output._glfwcontext = Glfw.GetApi();


            bool init = output._glfwcontext.Init();
            if (!init)
            {
                throw new Exception("Error while creating GLFW context");
            }

            //output.graphics_context.glfw_context = Glfw.GetApi();
            //bool init = output.graphics_context.glfw_context.Init();
            //if (!init)
            //{
            //    throw new Exception("Error while creating GLFW context");
            //}


            output._glfwcontext.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            output._glfwcontext.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            output._glfwcontext.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            output._glfwcontext.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            output._glfwcontext.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            output._glfwcontext.WindowHint(WindowHintBool.Resizable, true);
            output._glfwcontext.WindowHint(WindowHintBool.DoubleBuffer, true);
            output._glfwcontext.SetErrorCallback(OnGLFWError);

            switch (mode)
            {
                case WindowMode.FULLSCREEN:
                    CreateWindowFull(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED:
                    CreateWindowWindowed(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED_FULLSCREEN:
                    CreateWindowFullW(ref output, title, (int)width, (int)height);
                    break;
            }
            if (output.w_handle == null)
            {
                throw new Exception("Failed to create window");
            }


            output._glfwcontext.ShowWindow(output.w_handle);
            output._glfwcontext.MakeContextCurrent(output.w_handle);
            output._glcontext = GL.GetApi(output._glfwcontext.GetProcAddress);
            if (output._glcontext == null)
            {
                throw new Exception("Could not bind to GLFW PROC");
            }


            string[] devices = AudioContext.EnumSoundDevices();
            output.audio_context = AudioContext.Init("");

            output.w_size = new Vector2d((int)width, (int)height);
            output.w_title = title;
            output._glfwcontext.GetWindowPos(output.w_handle, out int x_pos, out int y_pos);
            output.w_position = new Vector2d(x_pos, y_pos);

            output.n_window = new GlfwNativeWindow(output._glfwcontext, output.w_handle);
            if (output.n_window.Win32.HasValue)
            {
                output._hwinst = output.n_window.Win32.Value.Hwnd;
            }
            else
            {
                throw new Exception("Could not capture native window ptr");
            }


            return output;

        }

        #region Window_Creation
        private static unsafe void CreateWindowWindowed(ref NativeWindow wnd, String title, int width, int height)
        {
            wnd.w_handle = wnd._glfwcontext.CreateWindow(width, height, title, null, null);
        }

        private static unsafe void CreateWindowFull(ref NativeWindow wnd, String title, int width, int height)
        {
            Monitor* mon = wnd._glfwcontext.GetPrimaryMonitor();
            wnd.w_handle = wnd._glfwcontext.CreateWindow(width, height, title, mon, null);
        }

        private static unsafe void CreateWindowFullW(ref NativeWindow wnd, String title, int width, int height)
        {
            Monitor* mon = wnd._glfwcontext.GetPrimaryMonitor();
            VideoMode* vidmode = wnd._glfwcontext.GetVideoMode(mon);

            wnd._glfwcontext.WindowHint(WindowHintInt.RedBits, vidmode->RedBits);
            wnd._glfwcontext.WindowHint(WindowHintInt.GreenBits, vidmode->GreenBits);
            wnd._glfwcontext.WindowHint(WindowHintInt.BlueBits, vidmode->BlueBits);
            wnd._glfwcontext.WindowHint(WindowHintInt.RefreshRate, vidmode->RefreshRate);
            wnd.w_handle = wnd._glfwcontext.CreateWindow(width, height, title, mon, null);
            
        }


        #endregion
        
        public unsafe void SetDrawArea(uint x, uint y, uint width, uint height)
        {
            _glcontext.Viewport((int)x, (int)y, width, height);
        }

        public unsafe void SwapBuffers()
        {
            _glfwcontext.SwapBuffers(w_handle);
        }

        public unsafe void ClearBuffer(ClearBufferMask mask)
        {
            _glcontext.Clear((uint)mask);
        }

        public unsafe void PollEvents()
        {
            _glfwcontext.PollEvents();
        }

        public static unsafe void OnGLFWError(Silk.NET.GLFW.ErrorCode error, string description)
        {
            Console.WriteLine($"GLFW encountered an error: {description}");
        }

        public unsafe void Destroy()
        {
            _glfwcontext.Terminate();
            audio_context.Destroy();
        }
    }

    public class AudioContext : IDisposable
    {
        public ALDevice sound_device;
        public ALContext context;

        private AudioContext()
        {
            sound_device = ALDevice.Null;
            context = ALContext.Null;
        }

        public static String[] EnumSoundDevices()
        {
            List<string> found = new List<string>(ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier));
            return found.ToArray();
        }

        public static String[] EnumCaptureDevices()
        {
            List<string> found = new List<string>(ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier));
            return found.ToArray();
        }

        public static unsafe AudioContext Init(string device_id)
        {
            AudioContext output = new AudioContext();
            output.sound_device = ALC.OpenDevice(device_id);
            if (output.sound_device.Handle == IntPtr.Zero)
            {
                throw new Exception("Could not create OpenAL audio device");
            }

            output.context = ALC.CreateContext(output.sound_device, (int*)null);
            if(output.context.Handle == IntPtr.Zero)
            {
                ALC.CloseDevice(output.sound_device);
                throw new Exception("Could not create OpenAL context");
            }
            
            bool succes = ALC.MakeContextCurrent(output.context);
            if(!succes)
            {
                ALC.DestroyContext(output.context);
                ALC.CloseDevice(output.sound_device);
                throw new Exception("Could not enable OpenAL context");
            }

            AL.IsExtensionPresent("SL_DATAFORMAT_PCM_EX");
            return output;
        }

        public void Destroy()
        {
            if(context != ALContext.Null && sound_device != ALDevice.Null)
            {
                ALC.DestroyContext(context);
                ALC.CloseDevice(sound_device);

                context.Handle = ALContext.Null;
                sound_device.Handle = ALDevice.Null;
            }
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
